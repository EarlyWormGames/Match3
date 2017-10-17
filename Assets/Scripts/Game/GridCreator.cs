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
    public GridLayoutGroup m_Layout;

    internal GridNode[,] m_Nodes;
    internal GridLayoutGroup m_Grid;
    internal Vector3[] m_ColumnSpawns;
    internal Vector3[] m_ColumnDistances;

    private int StartFrames = 5;


    // Use this for initialization
    void Start()
    {
        m_Grid = GetComponent<GridLayoutGroup>();

        m_GridWidth = Mediator.Settings.GridWidth;
        m_GridHeight = Mediator.Settings.GridHeight;

        m_Layout.constraintCount = m_GridWidth;

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

        int count = 0;
        do
        {
            //Initialise all nodes
            foreach (var node in m_Nodes)
            {
                node.Init();
            }

            if (count >= 100)
            {
                //After 100 tries, just give up
                break;
            }

            if (CheckColumns(true) == GameManager.instance.m_RequiredChainStart)
            {
                //We're done, break out of the do while
                break;
            }
            else
            {
                //If the chain count is wrong, try again
                foreach (var node in m_Nodes)
                {
                    node.DestroyNode();
                }
            }
            ++count;

        } while (true);

        GameManager.Stationary = new bool[m_GridWidth, m_GridHeight];
        GameManager.RespawnCounts = new int[m_GridWidth];
    }
    
    void Update()
    {
        if (GameManager.instance.m_IsGameOver) return;
        if (StartFrames > 0)
        {
            //DoOnce = true;
            for (int i = 0; i < m_GridWidth; ++i)
            {
                m_ColumnDistances[i] = (m_Nodes[i, 0].transform.position - m_Nodes[i, 1].transform.position);
                m_ColumnSpawns[i] = m_Nodes[i, 0].transform.position + m_ColumnDistances[i];
            }
            --StartFrames;
        }
        //GameManager.instance.m_Grid.CheckColumns();
    }

    //Normally never happens, just in case it does it reroutes it
    public void MouseUp(BaseEventData eventData)
    {
        if (GameManager.dragGNode == null)
            return;

        GameManager.dragGNode.MouseUp(eventData);
    }

    /// <summary>
    /// Check all nodes for matches
    /// </summary>
    public void MatchCheck()
    {
        foreach (var node in m_Nodes)
        {
            if (node.m_Shape != null)
            {
                ItemColour col = ItemColour.NONE;
                node.CheckMatch(Direction.None, ref col);
            }
        }
    }

    /// <summary>
    /// Check specific nodes for matches
    /// </summary>
    public void MatchCheck(GridNode[] a_nodes)
    {
        foreach (var node in a_nodes)
        {
            if (node.m_Shape != null)
            {
                ItemColour col = ItemColour.NONE;
                node.CheckMatch(Direction.None, ref col);
            }
        }
    }

    /// <summary>
    /// Fill the empty nodes by first dropping, then spawning new tiles
    /// </summary>
    public void FillEmpty()
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
                m_Nodes[i, j].SpawnTile(lastPos);
            }
            GameManager.RespawnCounts[i] = 0;
        }

        GameManager.instance.Refilled();
        CheckColumns();
    }

    /// <summary>
    /// This checks for the gameover state
    /// </summary>
    /// <param name="onlyCheck"></param>
    /// <returns></returns>
    public int CheckColumns(bool onlyCheck = false)
    {
        bool ok = false;
        int swapCount = 0;
        foreach (var node in m_Nodes)
        {
            if (node.m_Shape != null)
            {
                if (node.SwapCheckMatch())
                {
                    ok = true;
                    ++swapCount;
                    if (!onlyCheck)
                        break;
                }
            }
        }

        if (!ok)
        {
            if (!onlyCheck)
                GameManager.instance.GameOver(false);
            else
                return 0;
        }
        return swapCount;
    }
}