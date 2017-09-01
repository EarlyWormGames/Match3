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
                GameObject obj = Instantiate(m_NodePrefab);
                m_Nodes[j, i] = obj.GetComponent<GridNode>();
                obj.transform.parent = m_Grid.transform;
                obj.transform.localScale = m_Grid.transform.localScale;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
