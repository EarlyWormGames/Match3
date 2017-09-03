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
    internal Vector3[] m_ColumnSpawns;
    internal Vector3[] m_ColumnDistances;

    private bool DoOnce;


    // Use this for initialization
    void Start()
    {
        m_Grid = GetComponent<GridLayoutGroup>();

        m_Nodes = new GridNode[m_GridWidth, m_GridHeight];
        m_ColumnSpawns = new Vector3[m_GridWidth];
        m_ColumnDistances = new Vector3[m_GridWidth];
        for (int i = 0; i < m_GridHeight; ++i)
        {
            for (int j = 0; j < m_GridWidth; ++j)
            {
                //Spawn the prefab
                GameObject obj = Instantiate(m_NodePrefab);
                m_Nodes[j, i] = obj.GetComponent<GridNode>();
                //Child and size it correctly
                obj.transform.SetParent(m_Grid.transform, false);
                obj.transform.localScale = m_Grid.transform.localScale;
                obj.transform.localPosition = new Vector3();

                m_Nodes[j, i].m_xIndex = j;
                m_Nodes[j, i].m_yIndex = i;

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

        do
        {
            foreach (var node in m_Nodes)
            {
                node.Init();
            }

            if (CheckColumns(true))
                break;
            else
            {
                foreach (var node in m_Nodes)
                {
                    node.DestroyNode();
                }
            }

        } while (true);

        GameManager.Moving = new bool[m_GridWidth, m_GridHeight];
        GameManager.RespawnCounts = new int[m_GridWidth];
    }
    
    void Update()
    {
        if (!DoOnce)
        {
            //DoOnce = true;
            for (int i = 0; i < m_GridWidth; ++i)
            {
                m_ColumnDistances[i] = (m_Nodes[i, 0].transform.position - m_Nodes[i, 1].transform.position);
                m_ColumnSpawns[i] = m_Nodes[i, 0].transform.position + m_ColumnDistances[i];
            }
        }
        //GameManager.instance.m_Grid.CheckColumns();
    }

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

    public bool CheckColumns(bool onlyCheck = false)
    {
        bool ok = false;
        foreach (var node in m_Nodes)
        {
            if (node.m_Shape != null)
            {
                node.CheckMatch(Direction.None, onlyCheck);
            }
        }

        if (!onlyCheck)
        {
            for (int i = 0; i < m_GridWidth; ++i)
            {
                for (int j = m_GridHeight - 1; j >= 0; --j)
                {
                    if (m_Nodes[i, j].m_Shape == null)
                    {
                        m_Nodes[i, j].TryTakeUp();
                    }
                }
            }

            for (int i = 0; i < GameManager.RespawnCounts.Length; ++i)
            {
                Vector3 lastPos = m_ColumnSpawns[i] - m_ColumnDistances[i];

                for (int j = GameManager.RespawnCounts[i] - 1; j >= 0; --j)
                {
                    lastPos += m_ColumnDistances[i];
                    m_Nodes[i, j].SpawnShape(lastPos);
                }
                GameManager.RespawnCounts[i] = 0;
            }
        }

        foreach (var node in m_Nodes)
        {
            if (node.m_Shape != null)
            {
                if (node.SwapCheckMatch())
                {
                    ok = true;
                    break;
                }
            }
        }

        if (!ok)
        {
            if (!onlyCheck)
                GameManager.instance.GameOver();
            else
                return false;
        }
        return true;
    }
}
