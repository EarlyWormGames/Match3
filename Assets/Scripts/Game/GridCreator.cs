using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCreator : MonoBehaviour
{
    public GameObject m_NodePrefab;
    public int m_GridWidth = 6;
    public int m_GridHeight = 9;

    internal GridNode[,] m_Nodes;
    internal GridLayoutGroup m_Grid;


    // Use this for initialization
    void Start()
    {
        m_Grid = GetComponent<GridLayoutGroup>();

        m_Nodes = new GridNode[m_GridWidth, m_GridHeight];
        for (int i = 0; i < m_GridHeight; ++i)
        {
            for (int j = 0; j < m_GridWidth; ++j)
            {
                //Spawn the prefab
                GameObject obj = Instantiate(m_NodePrefab);
                m_Nodes[j, i] = obj.GetComponent<GridNode>();
                //Child and size it correctly
                obj.transform.parent = m_Grid.transform;
                obj.transform.localScale = m_Grid.transform.localScale;
                obj.transform.localPosition = new Vector3();

                if (j > 0)
                {
                    //Set the left/right nodes if they exist
                    m_Nodes[j, i].m_Left = m_Nodes[j - 1, i];
                    m_Nodes[j - 1, i].m_Right = m_Nodes[j, i];
                }
                if (i > 0)
                {
                    //Set the up/down nodes if they exist
                    m_Nodes[j, i].m_Up = m_Nodes[j, i - 1];
                    m_Nodes[j, i - 1].m_Down = m_Nodes[j, i];
                }
            }
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //
    //}
}
