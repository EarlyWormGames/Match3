using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemColour
{
    Blue,
    Green,
    Pink,
    Purple,
    Orange,
    Yellow,
    Red,
}

public class NodeItem : MonoBehaviour
{
    public ItemColour m_Colour;
    public ParticleSystem m_Explosion;
    public bool m_CanSwap = true;
    internal GridNode m_Parent;
    internal Direction m_SwappableDirection;
    internal Animator m_GemAnimator;
    internal bool MarkDestroy = false;

    private float m_DestroyTimer = 0;
    private bool m_destroyStart = false;

    // Use this for initialization
    void Start()
    {
        m_GemAnimator = GetComponent<Animator>();
        m_Explosion.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if ((m_Parent.transform.position - transform.position).magnitude > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, m_Parent.transform.position, Time.deltaTime * GameManager.instance.m_NodeMoveSpeed);
            if (!GameManager.isDragging)
                GameManager.Moving[m_Parent.m_xIndex, m_Parent.m_yIndex] = false;
        }
        else
        {
            if (!GameManager.isDragging)
                GameManager.Moving[m_Parent.m_xIndex, m_Parent.m_yIndex] = true;
        }

        if (m_destroyStart)
        {
            m_DestroyTimer -= Time.deltaTime;
            if (m_DestroyTimer <= 1)
            {
                if (GetComponent<MeshRenderer>() != null)
                    GetComponent<MeshRenderer>().enabled = false;
            }
            if (m_DestroyTimer <= 0)
            {
                m_destroyStart = false;
                m_Parent.EndDestroy();
            }
        }
    }

    /// <summary>
    /// Swap this node with another
    /// </summary>
    /// <param name="a_other">The other node to swap with</param>
    public void Swap(NodeItem a_other, Direction a_dir)
    {
        a_other.AssignSwap(a_dir);

        if (a_dir == Direction.Left)
            AssignSwap(Direction.Right);

        if (a_dir == Direction.Right)
            AssignSwap(Direction.Left);

        if (a_dir == Direction.Up)
            AssignSwap(Direction.Down);

        if (a_dir == Direction.Down)
            AssignSwap(Direction.Up);

        //Swap the shape inside the parent
        a_other.m_Parent.m_Shape = this;
        m_Parent.m_Shape = a_other;

        //Swap the parents
        GridNode temp = a_other.m_Parent;
        a_other.m_Parent = m_Parent;
        m_Parent = temp;

        //m_Parent.CheckMatch();
        //a_other.CheckMatch();
    }

    public void AssignSwap(Direction a_dir)
    {
        if (m_SwappableDirection == Direction.None)
            m_SwappableDirection = a_dir;
        else
            m_SwappableDirection = Direction.None;
    }

    public virtual void StartDestroy()
    {
        m_destroyStart = true;
        m_DestroyTimer = 1.5f;
        m_Explosion.Play();
    }

    public virtual void EndDestroy()
    {
        DestroyImmediate(gameObject);
    }
}
