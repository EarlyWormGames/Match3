using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BacteriaBro : NodeItem
{
    public int m_Lifespan = 3;
    public TextMeshProUGUI m_LifeText;
    public Image m_Sprite1;
    public Image m_Sprite2;
    public Image m_Sprite3;
    public ParticleSystem m_Sprite1Explosion;
    public ParticleSystem m_Sprite2Explosion;
    public ParticleSystem m_Sprite3Explosion;
    public GameObject m_ChildRoot;

    internal bool AllowCountUp = false;
    internal GameObject m_RespawnType;

    private int m_Lifetime;
    private bool m_WasReversed;
    private float m_FontStartSize;

    private bool m_CanDie = false;

    void Awake()
    {
        GameManager.onEOFSwap += NotifySwap;
        m_CanSwap = true;
        m_CanDestroy = false;
    }

    protected override void OnStart()
    {
        m_FontStartSize = m_LifeText.fontSize;
        LifeTimeDisplayUpdate();
    }

    protected override void OnUpdate()
    {
        m_CanDie = true;
    }

    /// <summary>
    /// Called when any tile has been swapped
    /// </summary>
    public void NotifySwap()
    {
        if (MarkDestroy || m_WasReversed || !m_CanDie)
        {
            m_WasReversed = false;
            return;
        }

        if (AllowCountUp)
            ++m_Lifetime;
        if (m_Lifetime >= m_Lifespan)
        {
            //Tell the node to destroy and be PETRIFIED
            NodeItem node = Instantiate(m_RespawnType).GetComponent<NodeItem>();
            node.m_CanSwap = false;
            node.m_CanDestroy = false;
            node.m_bPetrified = true;
            m_Parent.m_RespawnType = node.gameObject;
            m_Parent.m_RespawnIsSpawned = true;
            m_Parent.StartDestroy();
        }
        else
            LifeTimeDisplayUpdate();
    }

    protected override void OnNotifyDestroy()
    {
        if (MarkDestroy)
            return;

        m_WasReversed = true;
    }

    protected override void OnNotifyEndDestroy()
    {
        if (MarkDestroy || !m_CanDie)
            return;

        --m_Lifetime;
        LifeTimeDisplayUpdate();
        if (m_Lifetime == 0)
            m_Sprite3Explosion.Play();
        if (m_Lifetime == 1)
            m_Sprite2Explosion.Play();
        if (m_Lifetime < 0)
        {
            m_Sprite1Explosion.Play();
            m_Parent.m_RespawnType = m_RespawnType;
            m_Parent.StartDestroy();
        }
    }

    private void OnDestroy()
    {
        GameManager.onEOFSwap -= NotifySwap;
    }

    private void LifeTimeDisplayUpdate()
    {
        m_LifeText.text = (m_Lifetime + 1).ToString();
        m_LifeText.color = new Color((float)m_Lifetime / (m_Lifespan - 1), 0, 0);
        m_LifeText.fontSize = m_FontStartSize + m_Lifetime * 5;
        if (m_Lifetime >= 1)
        {
            m_Sprite2.gameObject.SetActive(true);
        }
        else
        {
            m_Sprite2.gameObject.SetActive(false);
        }
        if (m_Lifetime >= 2)
        {
            m_Sprite3.gameObject.SetActive(true);
        }
        else
        {
            m_Sprite3.gameObject.SetActive(false);
        }
    }

    protected override void OnDestroyMesh()
    {
        m_LifeText.gameObject.SetActive(false);
        m_Sprite1.gameObject.SetActive(false);
        m_Sprite2.gameObject.SetActive(false);
        m_Sprite3.gameObject.SetActive(false);
    }

    public override void OnEndDestroy()
    {
        //Do nothing
    }

    public void TakeBBInfo(BacteriaBro a_other)
    {
        m_Lifespan = a_other.m_Lifespan;
        m_LifeText = a_other.m_LifeText;
        m_Sprite1 = a_other.m_Sprite1;
        m_Sprite2 = a_other.m_Sprite2;
        m_Sprite3 = a_other.m_Sprite3;
        m_ChildRoot = a_other.gameObject;
    }

    void GoodDestroy()
    {
        Destroy(m_ChildRoot);
    }
}