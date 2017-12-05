using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    public delegate void MyDel();
    public delegate void BoolDel(bool a_bool);
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
    public static List<NodeItem> lastSwapped = new List<NodeItem>();
    //The direction of the last drag
    public static Direction lastDrag = Direction.None;
    public static Vector3 dragStartPos;
    public static int Score;

    public static int TopViewTime = 0;

    //An array of which Grid Nodes are moving
    public static bool[,] Stationary;
    public static List<GridNode> MovingTiles = new List<GridNode>();

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

    public static int LastChainCount = 0;
    public static int LastChainGoodCount = 0;

    public static int LastScore;

    //Delegate function for notifying a turn
    #region DELEGATES
    public static MyDel onNotifySwap;
    public static MyDel onEOFSwap;
    public static ColourDel onScored;
    public static BoolDel onRefill;
    #endregion

    #endregion

    [Header("UI Connections")]
    public Canvas m_MainCanvas;
    public GridCreator m_Grid;
    public GameObject m_ScorePanel;
    public PercentageMovement m_ScoreBar;
    public TextMeshProUGUI m_TurnsText;
    public GameObject m_WinPanel;
    public GameObject m_ArcadeWinPanel;
    public GameObject m_LosePanel;
    public StarShower m_Stars;
    public NumberScroller m_FinalScore;
    public Animator m_WBCA;

    [Header("Values")]
    public float m_NodeMoveSpeed = 5f;
    public int m_RequiredChainStart = 2;
    public int m_BadGuySpawnChance = 10;
    public bool ShowHints = true;
    public int m_WBCATurns = 5;

    [Header("Prefabs")]
    public GameObject[] m_SpawnPrefabs = new GameObject[0];
    public GameObject m_Petrified;
    public GameObject m_BBros;
    public GameObject m_RottenFood;
    public GameObject m_MelAnomaPrefab;
    public GameObject m_AshMaticPrefab;
    public GameObject m_DrDecayPrefab;
    public GameObject m_DJPrefab;

    internal int m_TurnsLeft;
    internal bool m_IsGameOver = false;
    internal bool m_TotalGameOver = false;

    private int m_WBCATurnsLeft;
    private bool m_bSetGameOver = false;
    private bool m_bSetSuccess;
    private bool m_bSetTotalGO;
    private bool m_WasMoving = true;
    private bool m_WasEmpty;
    private bool m_bSwapped;
    private int m_TurnsMade = 0;

    private float GameTimer;

    // Use this for initialization
    void Awake()
    {
        if (Mediator.Settings == null)
        {
            Mediator.Settings = new GameSettings();
        }

        m_TurnsLeft = Mediator.Settings.Turns;
        m_TurnsText.text = m_TurnsLeft.ToString();

        if (Mediator.Settings.isArcade)
        {
            m_TurnsText.text = 0.ToString();
        }
        else
        {
            Score = (int)(Mediator.Settings.TargetScore / 2f);
        }

        if (Mediator.Settings.SpawnObject != null)
            Instantiate(Mediator.Settings.SpawnObject);
    }

    // Update is called once per frame
    void Update()
    {
        GameTimer += Time.deltaTime;

        if (m_IsGameOver) return;

        //Set the score display
        m_ScoreBar.Percentage = (float)Score / Mediator.Settings.TargetScore;

        if (Score >= Mediator.Settings.TargetScore)
        {
            GameOver(true);
        }

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
                    ok = !m_Grid.MatchCheck(MovingTiles.ToArray());
                    MovingTiles.Clear();
                }

                if (m_bSetGameOver && ok)
                {
                    m_bSetGameOver = false;
                    DoGameOver(m_bSetSuccess, m_bSetTotalGO);
                }
            }
            else
            {
                //No movement allowed while tiles are falling
                CanDrag = false;
            }

            EyePanel.instance.SetImage(TopViewTime <= 0);
            //if (TopViewTime > 0)
            //{
            //    for (int i = 0; i < instance.m_Grid.m_GridWidth; ++i)
            //    {
            //        instance.m_Grid.m_Nodes[i, 0].OverrideVis(true);
            //    }
            //}
        }
    }

    public void GameOver(bool a_success, bool a_completeFailure = false)
    {
        m_bSetGameOver = true;
        m_bSetSuccess = a_success;
        m_bSetTotalGO = a_completeFailure;
    }

    private void DoGameOver(bool a_success, bool a_completeFailure = false)
    {
        //It's game-over man! Game-over!
        if (!m_IsGameOver)
        {
            Debug.Log("GAME OVER");
            m_IsGameOver = true;

            //m_ScorePanel.SetActive(true);
            //m_EndScore.text = Score.ToString();

            if (a_success)
            {
                //Misison passed! Respect+
                if (!Mediator.Settings.isArcade)
                    m_WinPanel.SetActive(true);
                else
                    m_ArcadeWinPanel.SetActive(true);

                if (Mediator.Settings.Level > -1)
                {
                    //Save the score to file
                    int finalscore = 1;

                    if (m_TurnsLeft >= (Mediator.Settings.Turns / 5))
                    {
                        finalscore = 2;
                    }

                    if (m_TurnsLeft >= (Mediator.Settings.Turns / 2))
                    {
                        finalscore = 3;
                    }

                    if (!Mediator.Settings.isArcade)
                    {
                        if (finalscore > SaveData.LevelScores[Mediator.Settings.Level])
                        {
                            SaveData.LevelScores[Mediator.Settings.Level] = finalscore;
                            SaveData.Save();
                        }
                    }

                    if (!Mediator.Settings.isArcade)
                    {
                        m_Stars.ShowStars(finalscore);

                        Analytics.CustomEvent("Game Won", new Dictionary<string, object>
                          {
                            { "level", Mediator.Settings.Level },
                            { "score", Score },
                            { "target-score", Mediator.Settings.TargetScore },
                            { "turns-left", m_TurnsLeft },
                            { "target-turns", Mediator.Settings.Turns},
                            { "final-score", finalscore },
                            { "Time-played", GameTimer}
                          });
                    }
                    else
                    {
                        float bonus = Mediator.Settings.ArcadeScore * (1 - (m_TurnsMade / (float)Mediator.Settings.Turns));
                        finalscore = (int)((Mediator.Settings.Level + 1) * Mathf.Max(0, bonus));
                        m_FinalScore.BeginScroll(finalscore);

                        SaveData.LastArcade = Mediator.Settings.Level;
                        SaveData.ArcadeScore += finalscore;
                        SaveData.Save();

                        Analytics.CustomEvent("Arcade Won", new Dictionary<string, object>
                          {
                            { "level", Mediator.Settings.Level },
                            { "score", Score },
                            { "target-score", Mediator.Settings.TargetScore },
                            { "turns-made", m_TurnsMade },
                            { "target-turns", Mediator.Settings.Turns},
                            { "final-score", finalscore },
                            { "Time-played", GameTimer}
                          });
                    }
                }
            }
            else
            {
                if (a_completeFailure)
                {
                    m_IsGameOver = false;
                    Instantiate(m_DJPrefab);

                    Analytics.CustomEvent("Reset Board", new Dictionary<string, object>
                      {
                        { "level", Mediator.Settings.Level },
                        { "Time-played", GameTimer}
                      });
                }
                else
                {
                    //Mission failed. We'll get 'em next time
                    m_LosePanel.SetActive(true);

                    Analytics.CustomEvent("Game Lost", new Dictionary<string, object>
                      {
                            { "level", Mediator.Settings.Level },
                            { "score", Score },
                            { "target-score", Mediator.Settings.TargetScore },
                            { "turns-left", m_TurnsLeft },
                            { "target-turns", Mediator.Settings.Turns},
                            { "Time-played", GameTimer}
                      });
                }
            }
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

    public void SubmitScore(string levelToLoad = "Menu")
    {
        //Once the game is over, submit the score to file
        GetComponent<HighScores>().AddScore(Score);
        GetComponent<HighScores>().SaveScoresToFile();

        //Disable the score panel and load a different scene
        m_ScorePanel.SetActive(false);
        GetComponent<Fading>().BeginFade(1, levelToLoad);

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
        MovingTiles = new List<GridNode>();
        CanDrag = true;
        TopViewTime = 0;
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

    public static void NotifyScore(ItemColour a_colour, GridNode a_node)
    {
        if (onScored != null)
            onScored(a_colour, instance.m_bSwapped, a_node);
    }

    public static void MarkSwap()
    {
        instance.m_bSwapped = true;
    }

    public void ShowWBCA()
    {
        if (m_WBCATurnsLeft <= 0)
            m_WBCA.SetTrigger("Show");
        m_WBCATurnsLeft = m_WBCATurns;
    }

    public void Refilled()
    {
        if (onRefill != null)
            onRefill(m_bSwapped);

        if (m_bSwapped)
        {
            if (m_WBCATurnsLeft == 1)
            {
                m_WBCA.SetTrigger("Hide");
            }

            ++m_TurnsMade;
            --m_TurnsLeft;
            --m_WBCATurnsLeft;

            if (Mediator.Settings.isArcade)
                m_TurnsText.text = m_TurnsMade.ToString();
            else
                m_TurnsText.text = m_TurnsLeft.ToString();

            if (m_TurnsLeft <= 0 && !Mediator.Settings.isArcade)
            {
                GameOver(false);
            }
            if (BadGuyUI.instance == null && m_TurnsMade > 3 && m_WBCATurnsLeft <= 0)
            {
                int rand = Random.Range(0, m_BadGuySpawnChance);
                if (rand == 0)
                {
                    rand = Random.Range(0, 3);
                    switch (rand)
                    {
                        case 0:
                            Instantiate(instance.m_MelAnomaPrefab);
                            break;
                        case 1:
                            Instantiate(instance.m_AshMaticPrefab);
                            break;
                        case 2:
                            Instantiate(instance.m_DrDecayPrefab);
                            break;
                    }
                }
            }
            else if (BadGuyUI.instance == null && m_WBCATurnsLeft > 0)
            {
                //Draw a Bad Guy but also draw a WBCA to stop them
                int rand = Random.Range(0, m_BadGuySpawnChance);
                BadGuy bg = null;
                if (rand == 0)
                {
                    rand = Random.Range(0, 3);
                    switch (rand)
                    {
                        case 0:
                            bg = Instantiate(instance.m_MelAnomaPrefab).GetComponent<BadGuy>();
                            break;
                        case 1:
                            bg = Instantiate(instance.m_AshMaticPrefab).GetComponent<BadGuy>();
                            break;
                        case 2:
                            bg = Instantiate(instance.m_DrDecayPrefab).GetComponent<BadGuy>();
                            break;
                    }

                    if (bg != null)
                        bg.NoEffect();
                }
            }
        }
        m_bSwapped = false;
    }

    public void ToggleHints()
    {
        ShowHints = !ShowHints;
    }
}