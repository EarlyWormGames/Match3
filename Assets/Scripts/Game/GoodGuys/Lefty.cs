using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lefty : NodeItem
{
    public GameObject m_MatchingNormal;

    public void Awake()
    {
        
    }

    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item.m_Shape.m_Colour == m_Colour && !item.m_Shape.MarkDestroy)
            {
                item.StartDestroy();
            }
        }
    }
}
