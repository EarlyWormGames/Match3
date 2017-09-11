using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Righty : NodeItem
{
    public GameObject m_MatchingNormal;

    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item.m_Shape.m_Colour == m_MatchedColour && !item.m_Shape.MarkDestroy)
            {
                item.m_RespawnType = m_MatchingNormal;
                item.StartDestroy(false);
            }
        }
    }
}
