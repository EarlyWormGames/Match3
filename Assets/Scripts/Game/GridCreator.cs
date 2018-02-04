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
    public NodeItem[] StartNodes;
    public List<GameObject> FillArray = new List<GameObject>();

    internal GridNode[,] m_Nodes;
    internal Vector3[] m_ColumnSpawns;
    internal Vector3[] m_ColumnDistances;

    private int StartFrames = 5;
    RectTransform parentRect;

    // Use this for initialization
    void Start()
    {
        if (StartNodes.Length < 1)
        {
            m_GridWidth = Mediator.Settings.GridWidth;
            m_GridHeight = Mediator.Settings.GridHeight;
        }

        parentRect = gameObject.GetComponent<RectTransform>();
        m_Layout.constraintCount = m_GridWidth;

        m_Nodes = new GridNode[m_GridWidth, m_GridHeight];
        m_ColumnSpawns = new Vector3[m_GridWidth];
        m_ColumnDistances = new Vector3[m_GridWidth];
        GameManager.Stationary = new bool[m_GridWidth, m_GridHeight];
        GameManager.RespawnCounts = new int[m_GridWidth];

        for (int i = 0; i < m_GridHeight; ++i)
        {
            for (int j = 0; j < m_GridWidth; ++j)
            {
                //Spawn the prefab
                GameObject obj = Instantiate(m_NodePrefab);
                m_Nodes[j, i] = obj.GetComponent<GridNode>();
                //Child and size it correctly
                obj.transform.SetParent(m_Layout.transform, false);
                obj.transform.localScale = m_Layout.transform.localScale;
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

        if (StartNodes.Length > 0)
        {
            int i = 0;
            foreach (var node in m_Nodes)
            {
                StartNodes[i].m_Parent = node;
                node.m_Shape = StartNodes[i];

                ++i;
            }
            return;
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
    }
    
    void Update()
    {
        m_Layout.cellSize = new Vector2(parentRect.rect.width / (float)m_GridWidth, parentRect.rect.height / (float)(m_GridHeight + 1));
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
    public bool MatchCheck(GridNode[] a_nodes)
    {
        bool ret = false;
        foreach (var node in a_nodes)
        {
            if (node.m_Shape != null)
            {
                ItemColour col = ItemColour.NONE;
                if (node.CheckMatch(Direction.None, ref col))
                    ret = true;
            }
        }
        return ret;
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
                if (FillArray.Count > 0)
                {
                    m_Nodes[i, j].SpawnTile(lastPos, FillArray[0]);
                    FillArray.RemoveAt(0);
                }
                else
                    m_Nodes[i, j].SpawnTile(lastPos);
            }
            GameManager.RespawnCounts[i] = 0;
        }

        GameManager.instance.Refilled();
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
                GameManager.instance.GameOver(false, true);
            else
                return 0;
        }
        return swapCount;
    }

    public void ResetBoard()
    {
        GameManager.DestroyingList.Clear();
        List<NodeItem> items = new List<NodeItem>();

        foreach (var node in m_Nodes)
        {
            items.Add(node.m_Shape);
            node.m_Shape.m_Parent = null;
            node.m_Shape = null;
        }

        foreach (var node in m_Nodes)
        {
            int i = Random.Range(0, items.Count);
            node.m_Shape = items[i];
            node.m_Shape.m_Parent = node;
            items.RemoveAt(i);
        }
    }
}