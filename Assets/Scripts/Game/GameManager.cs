using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
#region STATIC_VARS
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

    internal static bool isDragging;
    internal static GridNode dragGNode;
    internal static NodeItem dragNItem;
    //The direction of the last drag
    internal static Direction lastDrag = Direction.None;
    internal static Vector3 dragStartPos;
    internal static int Score;

    //An array of which Grid Nodes are moving
    internal static bool[,] Stationary;

    //A bunch of lists for matching nodes
    internal static List<GridNode> NodeChainLeft = new List<GridNode>(),
        NodeChainRight = new List<GridNode>(),
        NodeChainUp = new List<GridNode>(),
        NodeChainDown = new List<GridNode>();
    //A list of which nodes are about to be destroyed
    internal static List<GridNode> DestroyingList = new List<GridNode>();

    //Which grid columns need new tiles and how many
    internal static int[] RespawnCounts;
    internal static bool CanDrag = true;

    #endregion

    public GridCreator m_Grid;
    public Text m_ScoreText;
    public GameObject m_ScorePanel;
    public Text m_EndScore;
    public Text m_NameText;

    public float m_NodeMoveSpeed = 5f;
    public int m_RequiredChainStart = 2;

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

        //Don't bother check for matches while a piece is being dragged
        if (!isDragging)
        {
            bool ok = true;
            foreach (var move in Stationary)
            {
                //If any item in this array is false, a tile is moving
                if (!move)
                {
                    ok = false;
                    m_WasMoving = true;
                    break;
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
                    m_Grid.MatchCheck();
                }
            }
            else
            {
                //No movement allowed while tiles are falling
                CanDrag = false;
            }
        }

        //If we have items to destroy, this list will be filled
        if (DestroyingList.Count > 0)
        {
            m_WasEmpty = true;
            CanDrag = false;
        }
        else if (m_WasEmpty) //Enter when the list is empty, but just had items in it
        {
            m_Grid.FillEmpty();
            m_WasEmpty = false;
            CanDrag = true;
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
}
