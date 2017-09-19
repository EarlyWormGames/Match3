using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void MyDel();
    public delegate void ColourDel(ItemColour a_colour, bool a_swapped, GridNode a_node);

    #region STATIC_VARS
    #region STATIC_GETS
    //Grab the static instance of this item
    public static GameManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<GameManager>();
            }
            return m_Instance;
        }
    }
    private static GameManager m_Instance;

    public static int SpawnChanceMax
    {
        get
        {
            if (spawnMax == 0)
            {
                foreach (var item in instance.m_SpawnPrefabs)
                {
                    spawnMax += item.GetComponent<NodeItem>().m_SpawnChance;
                }
            }
            return spawnMax;
        }
    }
    private static int spawnMax;
#endregion

    public static bool isDragging;
    public static GridNode dragGNode;
    public static NodeItem dragNItem;
    //The direction of the last drag
    public static Direction lastDrag = Direction.None;
    public static Vector3 dragStartPos;
    public static int Score;

    //An array of which Grid Nodes are moving
    public static bool[,] Stationary;
    public List<GridNode> MovingTiles = new List<GridNode>();

    //A bunch of lists for matching nodes
    public static List<GridNode> NodeChainLeft = new List<GridNode>(),
        NodeChainRight = new List<GridNode>(),
        NodeChainUp = new List<GridNode>(),
        NodeChainDown = new List<GridNode>();
    //A list of which nodes are about to be destroyed
    public static List<GridNode> DestroyingList = new List<GridNode>();

    //Which grid columns need new tiles and how many
    public static int[] RespawnCounts;
    public static bool CanDrag = true;

    //Delegate function for notifying a turn

    #region DELEGATES
    public static MyDel onNotifySwap; 
    public static MyDel onEOFSwap;
    public static ColourDel onScored;
    #endregion

    #endregion

    public Canvas m_MainCanvas;
    public GridCreator m_Grid;
    public Text m_ScoreText;
    public GameObject m_ScorePanel;
    public Text m_EndScore;
    public Text m_NameText;

    public float m_NodeMoveSpeed = 5f;
    public int m_RequiredChainStart = 2;

    public GameObject[] m_SpawnPrefabs = new GameObject[0];
    public GameObject m_Petrified;
    public GameObject m_BBros;
    public GameObject m_MelAnomaPrefab;

    private bool m_WasMoving = true;
    private bool m_IsGameOver = false;
    private bool m_WasEmpty;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Set the score display
        m_ScoreText.text = Score.ToString();

        //If we have items to destroy, this list will be filled
        if (DestroyingList.Count > 0)
        {
            m_WasEmpty = true;
            CanDrag = false;
            return;
        }
        else if (m_WasEmpty) //Enter when the list is empty, but just had items in it
        {
            m_Grid.FillEmpty();
            m_WasEmpty = false;
            CanDrag = true;
        }

        //Don't bother check for matches while a piece is being dragged
        if (!isDragging)
        {
            bool ok = true;
            for (int x = 0; x < m_Grid.m_GridWidth; ++x)
            {
                for (int y = 0; y < m_Grid.m_GridHeight; ++y)
                {
                    //If any item in this array is false, a tile is moving
                    if (!Stationary[x, y])
                    {
                        ok = false;
                        m_WasMoving = true;

                        if (!MovingTiles.Contains(m_Grid.m_Nodes[x, y]))
                            MovingTiles.Add(m_Grid.m_Nodes[x, y]);
                    }
                }
            }

            if (ok)
            {
                //Only enters here if no piece is moving
                CanDrag = true; //Allow a drag to start now that no tiles are falling
                if (m_WasMoving)
                {
                    //Check for matches
                    m_WasMoving = false;
                    m_Grid.MatchCheck(MovingTiles.ToArray());
                    MovingTiles.Clear();
                }
            }
            else
            {
                //No movement allowed while tiles are falling
                CanDrag = false;
            }
        }        
    }

    public void GameOver()
    {
        //WAMP-WAMP, game over man!
        if (!m_IsGameOver)
        {
            Debug.Log("GAME OVER");
            m_IsGameOver = true;

            m_ScorePanel.SetActive(true);
            m_EndScore.text = Score.ToString();
        }
    }

    public static void ClearNodeChains()
    {
        //These are used to determine how many tiles are chained in each direction
        NodeChainLeft.Clear();
        NodeChainRight.Clear();
        NodeChainUp.Clear();
        NodeChainDown.Clear();
    }

    public static Direction GetOpposite(Direction a_dir)
    {
        //Returns the opposite direction (Left -> Right etc.)
        //Returns None (generally if None is the input)

        if (a_dir == Direction.Left)
            return Direction.Right;

        if (a_dir == Direction.Right)
            return Direction.Left;

        if (a_dir == Direction.Up)
            return Direction.Down;

        if (a_dir == Direction.Down)
            return Direction.Up;

        return Direction.None;
    }

    public void SubmitScore()
    {
        //Once the game is over, submit the score to file
        GetComponent<HighScores>().AddScore(Score);
        GetComponent<HighScores>().SaveScoresToFile();

        //Disable the score panel and load a different scene
        m_ScorePanel.SetActive(false);
        GetComponent<Fading>().BeginFade(1, "Menu");

        //Reset all these vars, since they are static
        isDragging = false;
        dragGNode = null;
        dragNItem = null;
        lastDrag = Direction.None;
        dragStartPos = Vector3.zero;
        Score = 0;
        NodeChainLeft = new List<GridNode>();
        NodeChainRight = new List<GridNode>();
        NodeChainUp = new List<GridNode>();
        NodeChainDown = new List<GridNode>();
        DestroyingList = new List<GridNode>();
        CanDrag = true;
    }

    /// <summary>
    /// Get a the index of a node item using their preset chances
    /// </summary>
    /// <returns></returns>
    public static int GetNodeIndex()
    {
        int chance = Random.Range(0, SpawnChanceMax);
        int prevChance = 0;
        int index = 0;
        foreach (var item in instance.m_SpawnPrefabs)
        {
            var node = item.GetComponent<NodeItem>();

            if (chance >= prevChance && chance < prevChance + node.m_SpawnChance)
            {
                //Spawn this node
                return index;
            }
            //Increase the low chance
            prevChance += node.m_SpawnChance;
            ++index;
        }

        //Will only get here if the spawn array is empty
        return -1;
    }

    /// <summary>
    /// Gets a copy of a node item without spawning it
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static NodeItem GetNodeDetails(int index)
    {
        return instance.m_SpawnPrefabs[index].GetComponent<NodeItem>();
    }

    public static GameObject SpawnNodeItem()
    {
        return Instantiate(instance.m_SpawnPrefabs[GetNodeIndex()]);
    }

    public static GameObject SpawnNodeItem(int index)
    {
        return Instantiate(instance.m_SpawnPrefabs[index]);
    }

    public static void NotifySwap()
    {
        if (onNotifySwap != null)
            onNotifySwap();
    }

    public static void NotifyScore(ItemColour a_colour, bool a_wasSwapped, GridNode a_node)
    {
        if (onScored != null)
            onScored(a_colour, a_wasSwapped, a_node);

        if (MelAnoma.instance == null && a_wasSwapped)
        {
            int rand = Random.Range(0, 10);
            if (rand == 0)
            {
                Instantiate(instance.m_MelAnomaPrefab);
            }
        }
    }
}