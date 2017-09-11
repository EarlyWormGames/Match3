using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BacteriaBro : NodeItem
{
    public int m_Lifespan = 3;
    public Text m_LifeText;
    public SpriteRenderer m_Sprite1;
    public SpriteRenderer m_Sprite2;
    public SpriteRenderer m_Sprite3;
    private int m_Lifetime;
    private bool m_WasReversed;

    private bool m_CanDie = false;

    void Awake()
    {
        GameManager.onEOFSwap += NotifySwap;
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

        ++m_Lifetime;
        if (m_Lifetime >= m_Lifespan)
        {
            //Mark a bunch of things to ready destruction
            GameManager.CanDrag = false;
            GameManager.DestroyingList.Add(m_Parent);

            //Tell the node to destroy
            MarkDestroy = true;
            m_Parent.m_RespawnType = GameManager.instance.m_Petrified;
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
        if (m_Lifetime < 0)
        {
            //Mark a bunch of things to ready destruction
            ++GameManager.RespawnCounts[m_Parent.m_xIndex];
            GameManager.CanDrag = false;
            GameManager.DestroyingList.Add(m_Parent);

            //Tell the node to destroy
            MarkDestroy = true;
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
        m_LifeText.fontSize = 120 + m_Lifetime * 25;
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
}