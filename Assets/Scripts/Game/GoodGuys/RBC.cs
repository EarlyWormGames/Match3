using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBC : MonoBehaviour
{
    public void DoIt()
    {
        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item.m_Shape == null)
                continue;
            //Finds all junk items and destroys them
            if (item.m_Shape.GetType() == typeof(JunkItem) && !item.m_Shape.MarkDestroy)
            {
                item.StartDestroy();
            }
        }
    }
}
