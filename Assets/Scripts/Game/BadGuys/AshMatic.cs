using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class AshMatic : MonoBehaviour
{    
    public GameObject m_RottenFood;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoIt()
    {
        int randX = Random.Range(0, Mediator.Settings.GridWidth);
        int randY = Random.Range(0, Mediator.Settings.GridHeight);

        GameManager.instance.m_Grid.m_Nodes[randX, randY].m_Shape.gameObject.AddComponent<RottenItem>();
        GameObject rottenObj = Instantiate(m_RottenFood);
        rottenObj.transform.SetParent(GameManager.instance.m_Grid.m_Nodes[randX, randY].m_Shape.transform, false);

        do
        {
            int randX2 = Random.Range(0, Mediator.Settings.GridWidth);
            int randY2 = Random.Range(0, Mediator.Settings.GridHeight);

            if (randX == randX2 && randY == randY2)
                continue;

            GameManager.instance.m_Grid.m_Nodes[randX2, randY2].m_Shape.gameObject.AddComponent<RottenItem>();
            rottenObj = Instantiate(m_RottenFood);
            rottenObj.transform.SetParent(GameManager.instance.m_Grid.m_Nodes[randX2, randY2].m_Shape.transform, false);
            break;

        } while (true);
    }
}
