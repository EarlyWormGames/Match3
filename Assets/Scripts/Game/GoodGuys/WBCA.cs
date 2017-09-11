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
            if (item.m_Shape.GetType() == typeof(BacteriaBro))
            {
                item.StartDestroy();
            }
        }
    }
}
