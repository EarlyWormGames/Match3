using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
#region STATIC_VARS
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
    internal static GridNode dragObject;
    internal static NodeItem dragShape;
    internal static Direction lastDrag = Direction.None;
    internal static Vector3 dragStartPos;
    internal static int Score;

    internal static bool[,] Moving;

    internal static List<GridNode> NodeChainLeft = new List<GridNode>(),
        NodeChainRight = new List<GridNode>(),
        NodeChainUp = new List<GridNode>(),
        NodeChainDown = new List<GridNode>();
    internal static List<GridNode> DestroyingList = new List<GridNode>();

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
        m_ScoreText.text = Score.ToString();

        if (!isDragging)
        {
            bool ok = true;
            foreach (var move in Moving)
            {
                if (!move)
                {
                    ok = false;
                    m_WasMoving = true;
                    break;
                }
            }

            if (ok)
            {
                if (m_WasMoving)
                {
                    m_WasMoving = false;
                    m_Grid.MatchCheck();
                }
            }
        }

        if (DestroyingList.Count > 0)
        {
            m_WasEmpty = true;
            CanDrag = false;
        }
        else if (m_WasEmpty)
        {
            m_Grid.FillEmpty();
            m_WasEmpty = false;
            CanDrag = true;
        }
    }

    public void GameOver()
    {
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
        NodeChainLeft.Clear();
        NodeChainRight.Clear();
        NodeChainUp.Clear();
        NodeChainDown.Clear();
    }

    public static Direction GetOpposite(Direction a_dir)
    {
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
        GetComponent<HighScores>().AddScore(Score);
        GetComponent<HighScores>().SaveScoresToFile();
        m_ScorePanel.SetActive(false);
        GetComponent<Fading>().BeginFade(1, "Menu");

        isDragging = false;
        dragObject = null;
        dragShape = null;
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
