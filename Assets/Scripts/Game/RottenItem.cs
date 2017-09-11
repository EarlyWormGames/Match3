using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenItem : NodeItem
{
    int m_DecayTime = 3;

    private void OnEnable()
    {
        GameManager.onEOFSwap += OnSwap;
    }

    private void OnDisable()
    {
        GameManager.onEOFSwap -= OnSwap;
    }

    void OnSwap()
    {
        if (m_DecayTime > 0)
            --m_DecayTime;
    }

    public override void OnEndDestroy()
    {
        if (m_DecayTime <= 0)
            --GameManager.Score;
        else
            ++GameManager.Score;
    }
}
