using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lungs : MonoBehaviour
{
    public void DoIt()
    {
        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item.m_Shape == null)
                continue;
            //Finds all rotten items and destroys them
            if (item.m_Shape.GetType() == typeof(RottenItem) && !item.m_Shape.MarkDestroy)
            {
                (item.m_Shape as RottenItem).m_WasDeleted = true;
                item.StartDestroy();
            }
        }
    }
}
