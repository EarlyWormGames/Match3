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
        //If we're far enough away from our parent node, we should move
        if ((m_Parent.transform.position - transform.position).magnitude > 0.01f)
        {
            //Use an ease-out lerp to move
            transform.position = Vector3.Lerp(transform.position, m_Parent.transform.position, Time.deltaTime * GameManager.instance.m_NodeMoveSpeed);

            //Tell the gamemanager we're moving
            if (!GameManager.isDragging)
                GameManager.Stationary[m_Parent.m_xIndex, m_Parent.m_yIndex] = false;
        }
        else
        {
            //Tell the gamemanager we're not moving
            if (!GameManager.isDragging)
                GameManager.Stationary[m_Parent.m_xIndex, m_Parent.m_yIndex] = true;
        }

        //Is this tile about to be destroyed?
        if (m_destroyStart)
        {
            //Destruction is done on a timer
            m_DestroyTimer -= Time.deltaTime;
            if (m_DestroyTimer <= 0.5f)
            {
                //Disable the mesh after a certain amount of time
                if (GetComponent<MeshRenderer>() != null)
                    GetComponent<MeshRenderer>().enabled = false;
            }
            if (m_DestroyTimer <= 0)
            {
                //Timer is done, destroy this object
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

    public virtual void StartDestroy()
    {
        m_destroyStart = true;
        m_DestroyTimer = 1f;
        m_Explosion.Play();
    }

    public virtual void EndDestroy()
    {
        DestroyImmediate(gameObject);
    }
}
