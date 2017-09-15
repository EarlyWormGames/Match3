using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Righty : NodeItem
{
    public GameObject m_MatchingNormal;

    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        if (!MarkSwap)
            return;

        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item == m_Parent)
                continue;
            if (item.m_Shape == null)
                continue;
            if (item.m_Shape.MarkDestroy || !item.m_Shape.CanDestroy())
                continue;

            if (item.m_Shape.m_Colour == m_MatchedColour)
            {
                item.m_RespawnType = m_MatchingNormal;
                item.StartDestroy(false);
            }
        }
    }
}
