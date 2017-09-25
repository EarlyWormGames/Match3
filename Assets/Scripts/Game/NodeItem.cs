using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemColour
{
    Blue,
    Green,
    Purple,
    Yellow,
    Red,
    NONE
}

public class NodeItem : MonoBehaviour
{
    public ItemColour m_Colour;
    public ParticleSystem m_Explosion;
    public bool m_CanSwap = true;
    public bool m_CanDestroy = true;
    public int m_SpawnChance = 1;
    public bool m_NotifiesDestroy = true;
    public bool m_MatchAnyColour = false;
    public bool m_SwapOnly = false;
    public Vector3 m_Scale = new Vector3(1, 1, 1);

    internal GridNode m_Parent;
    internal Animator m_GemAnimator;
    internal bool MarkDestroy = false;
    internal bool MarkSwap = false;
    internal bool MarkDrag = false;
    internal bool SwapChain = false;
    internal ItemColour m_MatchedColour;

    private float m_DestroyTimer = 0;
    private bool m_destroyStart = false;
    private int m_SwapCountDown = 1;

    // Use this for initialization
    void Start()
    {
        foreach (var item in GetComponents<NodeItem>())
        {
            if (item != this)
            {
                Destroy(item); //Destroy anything that isn't this script. Useful for inherited scripts
                if (m_Parent == null && item.m_Parent != null)
                {
                    TakeNodeInfo(item);
                }
            }
        }

        m_GemAnimator = GetComponent<Animator>();
        if(m_Explosion != null)
        m_Explosion.Stop();
    }

    public virtual void Init() { }

    public void TakeNodeInfo(NodeItem item)
    {
        m_Colour = item.m_Colour;
        m_Parent = item.m_Parent;
        m_Parent.m_Shape = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Parent == null)
            return;

        //If we're far enough away from our parent node, we should move
        if ((m_Parent.transform.position - transform.position).magnitude > 0.01f)
        {
            //Use an ease-out lerp to move
            transform.position = Vector3.Lerp(transform.position, m_Parent.transform.position, Time.deltaTime * GameManager.instance.m_NodeMoveSpeed);

            //Tell the gamemanager we're moving
            if (!GameManager.isDragging)
                GameManager.Stationary[m_Parent.m_xIndex, m_Parent.m_yIndex] = false;

            if (MarkSwap)
                m_SwapCountDown = 1;
        }
        else
        {
            //Tell the gamemanager we're not moving
            if (!GameManager.isDragging)
                GameManager.Stationary[m_Parent.m_xIndex, m_Parent.m_yIndex] = true;

            if (m_SwapCountDown <= 0 && !m_destroyStart)
                MarkSwap = false;
            else
                --m_SwapCountDown;
        }        

        //Is this tile about to be destroyed?
        if (m_destroyStart)
        {
            //Destruction is done on a timer
            m_DestroyTimer -= Time.deltaTime;
            if (m_DestroyTimer <= 0.5f)
            {
                //Disable the mesh after a certain amount of time
                DestroyMesh();
            }
            if (m_DestroyTimer <= 0)
            {
                //Timer is done, destroy this object
                m_destroyStart = false;
                m_Parent.EndDestroy();
            }
        }

        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    public void DestroyMesh()
    {
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = false;
        OnDestroyMesh();
    }

    protected virtual void OnDestroyMesh() { }

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

    public void StartDestroy()
    {
        m_destroyStart = true;
        m_DestroyTimer = 1f;
        if(m_Explosion != null)
        m_Explosion.Play();

        if (m_NotifiesDestroy)
        {
            if (m_Parent.HasDirection(Direction.Left, true))
                m_Parent.m_Left.m_Shape.NotifyDestroy();
            if (m_Parent.HasDirection(Direction.Right, true))
                m_Parent.m_Right.m_Shape.NotifyDestroy();
            if (m_Parent.HasDirection(Direction.Up, true))
                m_Parent.m_Up.m_Shape.NotifyDestroy();
            if (m_Parent.HasDirection(Direction.Down, true))
                m_Parent.m_Down.m_Shape.NotifyDestroy();
        }

        OnStartDestroy();
    }

    public void EndDestroy()
    {
        if (m_NotifiesDestroy)
        {
            if (m_Parent.HasDirection(Direction.Left, true))
                m_Parent.m_Left.m_Shape.NotifyEndDestroy();
            if (m_Parent.HasDirection(Direction.Right, true))
                m_Parent.m_Right.m_Shape.NotifyEndDestroy();
            if (m_Parent.HasDirection(Direction.Up, true))
                m_Parent.m_Up.m_Shape.NotifyEndDestroy();
            if (m_Parent.HasDirection(Direction.Down, true))
                m_Parent.m_Down.m_Shape.NotifyEndDestroy();
        }

        if (GameManager.onEOFSwap != null && MarkSwap)
            GameManager.onEOFSwap();

        OnEndDestroy();

        Destroy(gameObject);
    }

    public virtual void OnStartDestroy() { }
    public virtual void OnEndDestroy()
    {
        ++GameManager.Score;
    }

    public virtual bool CanSwap()
    {
        return m_CanSwap;
    }

    public virtual bool CanDestroy()
    {
        return m_CanDestroy;
    }

    /// <summary>
    /// Called when an adjacent tile is destroyed
    /// </summary>
    public void NotifyDestroy()
    {
        if (MarkDestroy)
            return;
        OnNotifyDestroy();
    }

    public void NotifyEndDestroy()
    {
        if (MarkDestroy)
            return;
        OnNotifyEndDestroy();
    }

    protected virtual void OnNotifyDestroy() { }
    protected virtual void OnNotifyEndDestroy() { }

    public virtual bool CheckColour(NodeItem a_node, ItemColour a_col, bool a_override = false)
    {
        bool wegoodcuh = true;
        if (m_SwapOnly)
            wegoodcuh = MarkSwap || MarkDrag || a_override;

        if (a_node.m_SwapOnly)
            wegoodcuh = a_node.MarkSwap || a_node.MarkDrag || a_override;

        return (m_Colour == a_col || a_col == ItemColour.NONE) && wegoodcuh;
    }
}