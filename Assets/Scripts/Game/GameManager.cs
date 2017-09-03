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
    internal static ItemColour LastColour;
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

    internal static int[] RespawnCounts;

    #endregion

    public GridCreator m_Grid;
    public Text m_ScoreText;
    public float m_NodeMoveSpeed = 5f;
    public int m_RequiredChainStart = 2;

    private bool m_WasMoving = true;
    private bool m_IsGameOver = false;

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
                    m_Grid.CheckColumns();
                }
            }
        }
    }

    public void GameOver()
    {
        if (!m_IsGameOver)
        {
            Debug.Log("GAME OVER");
            m_IsGameOver = true;
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
}
