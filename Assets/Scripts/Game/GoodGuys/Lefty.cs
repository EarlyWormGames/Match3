using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lefty : NodeItem
{
    public GameObject m_MatchingNormal;

    public void Awake()
    {
        
    }

    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        //Go through all the nodes in the grid
        foreach (var item in GameManager.instance.m_Grid.m_Nodes)
        {
            if (item.m_Shape == null || item.m_yIndex == 0)
                continue;

            //If it has the same colour as this and isn't already being destroyed
            if (item.m_Shape.m_Colour == m_Colour && !item.m_Shape.MarkDestroy)
            {
                //Destroy it!
                item.StartDestroy();
            }
        }
    }
}
