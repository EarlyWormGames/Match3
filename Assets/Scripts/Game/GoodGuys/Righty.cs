using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Righty : NodeItem
{
    public GameObject m_MatchingNormal;

    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        //SWAP ONLY :angry_emoji:
        if (!MarkSwap)
            return;

        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item == m_Parent || item.m_Shape == null)
                continue;
            if (item.m_Shape.MarkDestroy || !item.m_Shape.CanDestroy())
                continue;

            //If it has the same colour as the one we matched with
            if (item.m_Shape.m_Colour == m_MatchedColour)
            {
                //Destroy it and give it a new item
                item.m_RespawnType = m_MatchingNormal;
                item.StartDestroy(false);
                GameManager.Stationary[item.m_xIndex, item.m_yIndex] = false;
                GameManager.MovingTiles.Add(item);
            }
        }
    }
}
