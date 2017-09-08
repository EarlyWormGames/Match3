using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BacteriaBro : NodeItem
{
    public int m_Lifespan = 3;
    public Text m_LifeText;
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
            m_Parent.StartDestroy();
            m_Parent.m_SpawnPetrified = true;
        }
        else
            m_LifeText.text = (m_Lifetime + 1).ToString();
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
        m_LifeText.text = (m_Lifetime + 1).ToString();
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
}