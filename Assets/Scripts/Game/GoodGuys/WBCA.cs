using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCA : MonoBehaviour
{
    public void DoIt()
    {
        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item.m_Shape.GetType() == typeof(BacteriaBro) && !item.m_Shape.MarkDestroy)
            {
                item.StartDestroy();
            }
        }
    }
}
