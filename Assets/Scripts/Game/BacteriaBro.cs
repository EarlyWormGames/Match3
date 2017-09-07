using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacteriaBro : NodeItem
{
    public int m_Lifespan = 3;
    private int m_Lifetime;

    void Awake()
    {
        GameManager.onEOFSwap += NotifySwap;
    }

    /// <summary>
    /// Called when any tile has been swapped
    /// </summary>
    public void NotifySwap()
    {
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
    }

    protected override void OnNotifyDestroy()
    {
        --m_Lifetime;
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