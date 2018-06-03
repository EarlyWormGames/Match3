using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBC : GoodNode
{
    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item.m_Shape == null || item.m_yIndex == 0)
                continue;
            //Finds all junk items and destroys them
            if (item.m_Shape.GetType() == typeof(JunkItem) && !item.m_Shape.MarkDestroy)
            {
                item.StartDestroy(false);
            }
        }
    }
}
