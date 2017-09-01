using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public void MouseUp(BaseEventData eventData)
    {
        if (!GameManager.isDragging)
            return;
        
        //Calculate the direction of the drag
        Vector3 dir = Input.mousePosition - GameManager.dragStartPos;
        dir.Normalize();

        //Get the node the drag started on
        GridNode node = GameManager.dragObject.GetComponent<GridNode>();

        //Convert these values to absolute values
        float x = dir.x;
        if (x < 0)
            x *= -1;
        float y = dir.y;
        if (y < 0)
            y *= -1;

        if (x > y)
        {
            if (dir.x < 0)
            {
                //Do left
                node.TrySwap(Direction.Left);
            }
            else
            {
                //Do right
                node.TrySwap(Direction.Right);
            }
        }
        else
        {
            if (dir.y < 0)
            {
                //Do down
                node.TrySwap(Direction.Down);
            }
            else
            {
                //Do up
                node.TrySwap(Direction.Up);
            }
        }

        //Finished, reset this data
        GameManager.dragStartPos = Vector3.zero;
        GameManager.isDragging = false;
        GameManager.dragObject = null;
    }

    public void CheckColumns()
    {
        for (int i = 0; i < m_GridWidth; ++i)
        {
            m_Nodes[i, m_GridHeight - 1].CheckColumn();
        }
    }
}
