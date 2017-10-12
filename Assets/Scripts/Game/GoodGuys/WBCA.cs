using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCA : NodeItem
{
    public override void OnEndDestroy()
    {
        base.OnEndDestroy();
        
        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item.m_Shape == null || item.m_yIndex == 0)
                continue;
            //Finds all Bacteria Bros and destroys them
            if (item.m_Shape.GetType() == typeof(BacteriaBro) && !item.m_Shape.MarkDestroy)
            {
                item.StartDestroy();
            }
        }
    }
}
