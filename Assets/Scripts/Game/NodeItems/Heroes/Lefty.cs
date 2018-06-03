using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lefty : GoodNode
{
    public ParticleSystem m_ConnectorParticle;
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
                GameObject ps = Instantiate(m_ConnectorParticle.gameObject, transform.parent);
                ps.transform.position = transform.position;
                ps.transform.LookAt(item.transform, Vector3.forward);
                ps.transform.Rotate(0, -90, 0);
                var shape = ps.GetComponent<ParticleSystem>().shape;
                float distance = Vector3.Distance(transform.position, item.transform.position)/2;
                shape.radius = distance;
                shape.position = new Vector3(distance, 0, 0);
                Destroy(ps, 0.4f);
                //Destroy it!
                item.StartDestroy();
            }
        }
    }
}
