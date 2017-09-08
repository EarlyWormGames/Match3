using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BacteriaBro : NodeItem
{
    public int m_Lifespan = 3;
    public Text m_LifeText;
    public SpriteRenderer m_BBSprite2;
    public SpriteRenderer m_BBSprite3;
    public SpriteRenderer m_Sprite;
    private int m_Lifetime;
    private bool m_WasReversed;

    void Awake()
    {
        GameManager.onEOFSwap += NotifySwap;
        m_LifeText.text = (m_Lifetime + 1).ToString();
    }

    /// <summary>
    /// Called when any tile has been swapped
    /// </summary>
    public void NotifySwap()
    {
        if (MarkDestroy || m_WasReversed)
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
        if (MarkDestroy)
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
        if (m_Lifetime >= 1)
        {
            m_BBSprite2.gameObject.SetActive(true);
        }
        else
        {
            m_BBSprite2.gameObject.SetActive(false);
        }
        if (m_Lifetime >= 2)
        {
            m_BBSprite3.gameObject.SetActive(true);
        }
        else
        {
            m_BBSprite3.gameObject.SetActive(false);
        }
    }
    protected override void OnDestroyMesh()
    {
        m_LifeText.gameObject.SetActive(false);
        m_Sprite.gameObject.SetActive(false);
    }
}