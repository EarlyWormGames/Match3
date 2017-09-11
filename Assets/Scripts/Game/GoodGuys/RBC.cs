using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBC : MonoBehaviour
{
    public void DoIt()
    {
        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item.m_Shape.GetType() == typeof(JunkItem))
            {
                item.StartDestroy();
            }
        }
    }
}
