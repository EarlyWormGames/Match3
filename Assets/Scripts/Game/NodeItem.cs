using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemColour
{
    Broccoli,
    Cake,
    Capsicum,
    Carrot,
    Cupcake,
    Icecream,
    NONE
}

public class NodeItem : MonoBehaviour
{
    public ItemColour m_Colour;
    public ParticleSystem m_Explosion;
    public ParticleSystem m_AfterExplosion;
    public float m_AfterEXDestroyTime = 1;
    public bool m_RelativeAfterEX = true;
    public bool m_CanSwap = true;
    public bool m_CanDestroy = true;
    public int m_SpawnChance = 1;
    public bool m_NotifiesDestroy = true;
    public bool m_MatchAnyColour = false;
    public bool m_MatchAnyButOwn = false;
    public bool m_SwapOnly = false;
    public Vector3 m_Scale = new Vector3(1, 1, 1);

    internal GridNode m_Parent;
    internal Animator m_GemAnimator;
    internal bool MarkDestroy = false;
    internal bool MarkSwap = false;
    internal bool MarkDrag = false;
    internal bool SwapChain = false;
    internal ItemColour m_MatchedColour;
    internal bool m_bIsActive = true;
    internal bool m_bUseScore = true;

    private float m_DestroyTimer = 0;
    private bool m_destroyStart = false;
    private int m_SwapCountDown = 1;
    private bool m_bWasMoving;

    private Image[] m_ImageComps = new Image[0];
    private SpriteRenderer[] m_SpriteComps = new SpriteRenderer[0];
    private ParticleSystem[] m_Particles = new ParticleSystem[0];

    private void Awake()
    {
        m_ImageComps = GetComponentsInChildren<Image>();
        m_SpriteComps = GetComponentsInChildren<SpriteRenderer>();
        m_Particles = GetComponentsInChildren<ParticleSystem>();
    }

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

        OnStart();
    }

    protected virtual void OnStart() { }

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
        if (GameManager.instance.m_IsGameOver) return;
        if (m_Parent == null)
            return;

        m_bIsActive = m_Parent.m_yIndex != 0;

        //If we're far enough away from our parent node, we should move
        if ((m_Parent.transform.position - transform.position).magnitude > 0.01f)
        {
            //We'll use this later once this tile stops moving
            m_bWasMoving = true;

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
            {
                GameManager.Stationary[m_Parent.m_xIndex, m_Parent.m_yIndex] = true;

                if (MarkDrag)
                {

                }
            }

            if (m_SwapCountDown <= 0 && !m_destroyStart)
                MarkSwap = false;
            else
                --m_SwapCountDown;

            if (m_bWasMoving)
            {
                m_bWasMoving = false;

                //For animations and such
                OnStopMovement();
            }
        }        

        //Is this tile about to be destroyed?
        if (m_destroyStart || MarkDestroy)
        {
            //Destruction is done on a timer
            m_DestroyTimer -= Time.deltaTime;
            if (GameManager.DestroyingList.Count >= 4)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2f, 1f, 1), Time.deltaTime * 3);
            }
            if (m_DestroyTimer <= (GameManager.DestroyingList.Count >= 4 ? -0.05f : 0.1f))
            {
                //Disable the mesh after a certain amount of time
                DestroyMesh();
            }
            if (m_DestroyTimer <= (GameManager.DestroyingList.Count >= 4 ? -0.15f : 0f))
            {
                //Timer is done, destroy this object
                m_destroyStart = false;
                if (m_AfterExplosion != null)
                {
                    GameObject AfterEX = Instantiate(m_AfterExplosion, m_RelativeAfterEX ? m_Parent.transform : null).gameObject;
                    if(AfterEX.GetComponent<ParticleColourChanger>() != null)
                    {
                        AfterEX.GetComponent<ParticleColourChanger>().m_gemColour = m_Colour;
                    }
                    Destroy(AfterEX, m_AfterEXDestroyTime);
                }
                m_Parent.EndDestroy();
            }
        }

        OnUpdate();
    }

    public void OverrideVis(bool a_vis)
    {
        foreach (var img in m_ImageComps)
        {
            img.enabled = a_vis;
        }

        foreach (var sprite in m_SpriteComps)
        {
            sprite.enabled = a_vis;
        }

        foreach (var part in m_Particles)
        {
            if (part.main.playOnAwake && a_vis)
                part.Play();
            else
                part.Stop();
        }
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

    public void StartDestroy(bool a_useScore = true)
    {
        m_destroyStart = true;
        
        if(m_Explosion != null)
        m_Explosion.Play();
        
        m_DestroyTimer = 0.25f;

        m_bUseScore = a_useScore;

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
        if (m_bUseScore)
            ++GameManager.Score;
    }

    public virtual bool CanSwap()
    {
        return m_CanSwap && m_bIsActive;
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

        if (a_col != ItemColour.NONE)
        {
            if (a_node.m_MatchAnyButOwn && a_col == a_node.m_Colour ||
                m_MatchAnyButOwn && a_col == m_Colour)
            {
                wegoodcuh = false;
            }
        }

        if ((a_node.m_SwapOnly && a_node.m_MatchAnyButOwn) ||
            (m_SwapOnly && m_MatchAnyButOwn))
            wegoodcuh = false;

        if (!m_bIsActive || !a_node.m_bIsActive)
            wegoodcuh = false;

        return (m_Colour == a_col || a_col == ItemColour.NONE) && wegoodcuh;
    }

    protected virtual void OnStopMovement() { }
}