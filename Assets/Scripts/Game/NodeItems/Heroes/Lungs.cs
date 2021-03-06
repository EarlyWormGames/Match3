﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lungs : GoodNode
{
    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item.m_Shape == null || item.m_yIndex == 0)
                continue;
            //Finds all rotten items and destroys them
            if (item.m_Shape.GetType() == typeof(RottenItem) && !item.m_Shape.MarkDestroy)
            {
                item.m_Shape.m_AfterExplosion = item.m_Shape.GetComponent<RottenItem>().m_CleaningExplosion;
                item.StartDestroy(false);
            }
        }
    }
}
