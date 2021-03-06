﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;
using System.Linq;

[System.Serializable]
public class IntEvent : UnityEvent<int> { }

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

    #endregion

    //Delegate functions for notifying a turn
    #region DELEGATES
    public static MyDel onNotifySwap;
    public static MyDel onEOFSwap;
    public static MyDel onGameOver;
    public static ColourDel onScored;
    public static BoolDel onRefill;
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
    public IntEvent m_ShowBadGuyPage;

    [Header("Sounds")]
    public AudioSource m_ScoreSound;
    public AudioSource m_LosePointSound;
    public AudioSource m_WhooshSound;

    [Header("Values")]
    public float m_NodeMoveSpeed = 5f;
    public int m_RequiredChainStart = 2;
    [Range(0, 100)] public int m_BadGuySpawnChance = 10;
    [Tooltip("The minimum amount of turns to pass before a Bad Guy can spawn")]
    public int MinimumTurnsBeforeEnemy = 3;
    public bool ShowHints = true;
    public int m_WBCATurns = 5;
    public int Score;

    [Header("Prefabs")]
    public GameObject[] m_SpawnPrefabs = new GameObject[0];
    public GameObject m_BBros;
    public GameObject m_RottenFood;
    public GameObject[] m_BadGuyPrefabs = new GameObject[0];
    public GameObject m_DJPrefab;

    public Sprite[] m_GridSprites;

    public bool UIBlocked { get; set; }
    [HideInInspector]
    public bool trueUIBlocked;

    internal int m_TurnsLeft;
    internal bool m_IsGameOver = false;
    internal bool m_TotalGameOver = false;
    internal bool m_bAllowRegularGameOver = true;

    private int m_WBCATurnsLeft;
    private bool m_bSetGameOver = false;
    private bool m_bSetSuccess;
    private bool m_bSetTotalGO;
    private bool m_WasMoving = true;
    private bool m_WasEmpty;
    private bool m_bSwapped;
    private int m_TurnsMade = 0;
    private bool m_BadGuyShown;

    private float GameTimer;

    private bool wasRefilled;

    // Use this for initialization
    void Awake()
    {
        AdManager.AdAllowed = true; //Enable ads again

        if (Mediator.Settings == null)
        {
            Mediator.Settings = new GameSettings();
        }

        m_TurnsLeft = Mediator.Settings.Turns;
        m_TurnsText.text = m_TurnsLeft.ToString();

        if (Mediator.Settings.isArcade)
        {
            m_TurnsText.text = "0";
        }
        else
        {
            Score = (int)(Mediator.Settings.TargetScore / 2f);
        }

        //Remove the blacklisted items
        m_SpawnPrefabs = m_SpawnPrefabs.Except(Mediator.Settings.BlacklistedSpawns).ToArray();
        m_BadGuyPrefabs = m_BadGuyPrefabs.Except(Mediator.Settings.BlacklistedEnemies).ToArray();

        if (Mediator.Settings.SpawnObject != null)
            Instantiate(Mediator.Settings.SpawnObject);
    }

    // Update is called once per frame
    void Update()
    {
        trueUIBlocked = UIBlocked;

        GameTimer += Time.deltaTime;

        if (m_IsGameOver) return;

        //Set the score display
        m_ScoreBar.SetPercentage((float)Score / Mediator.Settings.TargetScore);

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
                bool noMatches = true;
                if (m_WasMoving)
                {
                    //Check for matches
                    m_WasMoving = false;
                    noMatches = !m_Grid.MatchCheck(MovingTiles.ToArray());
                    MovingTiles.Clear();
                }

                if (noMatches && wasRefilled)
                {
                    wasRefilled = false;
                    m_Grid.CheckColumns();
                }

                if (m_bSetGameOver && noMatches)
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

            if (a_success && onGameOver != null)
                onGameOver();

            //m_ScorePanel.SetActive(true);
            //m_EndScore.text = Score.ToString();

            if (m_bAllowRegularGameOver)
            {
                if (a_success)
                {
                    GameSuccess();
                }
                else
                {
                    if (a_completeFailure)
                    {
                        isDragging = true;
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
    }

    public void GameSuccess()
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

            if (Mediator.Settings.Level < SaveData.instance.LevelScores.Count && !Mediator.Settings.isArcade)
            {
                if (finalscore > SaveData.instance.LevelScores[Mediator.Settings.Level])
                {
                    SaveData.instance.LevelScores[Mediator.Settings.Level] = finalscore;
                    SaveData.Save();
                }
            }
            else if (!Mediator.Settings.isArcade)
            {
                SaveData.instance.LevelScores.Add(finalscore);
                SaveData.Save();
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

                SaveData.instance.LastArcade = Mediator.Settings.Level;
                SaveData.instance.ArcadeScore += finalscore;
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

                if (Facebook.Unity.FB.IsLoggedIn)
                {
                    FBAppEvents.GameComplete(SaveData.instance.ArcadeScore);
                    FBHighscores.SetScore(SaveData.instance.ArcadeScore);
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
        wasRefilled = true;

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
            --TopViewTime;

            if (Mediator.Settings.isArcade)
                m_TurnsText.text = m_TurnsMade.ToString();
            else
                m_TurnsText.text = m_TurnsLeft.ToString();

            if (m_TurnsLeft <= 0 && !Mediator.Settings.isArcade)
            {
                GameOver(false);
            }
            // Change turns made to  --------------------> 3
            if ((BadGuyUI.instance == null && m_TurnsMade > MinimumTurnsBeforeEnemy
                && m_WBCATurnsLeft <= 0 && !m_IsGameOver && !m_bSetGameOver) || (Mediator.Settings.ShowBadGuyAfter >= 0 && m_TurnsMade > Mediator.Settings.ShowBadGuyAfter))
            {
                if (Mediator.Settings.ShowBadGuyAfter >= 0 && Mediator.Settings.BadGuyToShow && !m_BadGuyShown)
                {
                    UIBlocked = true;
                    Instantiate(Mediator.Settings.BadGuyToShow);
                    m_ShowBadGuyPage.Invoke(Mediator.Settings.BadGuyPage);
                    m_BadGuyShown = false;
                }
                else
                {
                    int rand = Random.Range(0, m_BadGuySpawnChance);
                    if (rand == 0 && m_BadGuyPrefabs.Length > 0)
                    {
                        rand = Random.Range(0, m_BadGuyPrefabs.Length);
                        Instantiate(m_BadGuyPrefabs[rand]);
                    }
                }
            }
            else if (BadGuyUI.instance == null && m_WBCATurnsLeft > 0 && !m_IsGameOver && !m_bSetGameOver)
            {
                //Draw a Bad Guy but also draw a WBCA to stop them
                int rand = Random.Range(0, m_BadGuySpawnChance);
                BadGuy bg = null;
                if (rand == 0 && m_BadGuyPrefabs.Length > 0)
                {
                    rand = Random.Range(0, m_BadGuyPrefabs.Length);
                    bg = Instantiate(m_BadGuyPrefabs[rand]).GetComponent<BadGuy>();
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

    public void AddScore(int amount)
    {
        Score += amount;
        if (amount > 0)
            m_ScoreSound.PlayOneShot(m_ScoreSound.clip);
        else
            m_LosePointSound.PlayOneShot(m_LosePointSound.clip);
    }

    public static void Whoosh()
    {
        instance.m_WhooshSound.PlayOneShot(instance.m_WhooshSound.clip);
    }

    /// <summary>
    /// Randomly replaces a tile on the board with a prefab
    /// </summary>
    /// <param name="prefabToSpawn"></param>
    public static void ReplaceRandomTile(GameObject prefabToSpawn)
    {
        int randX = Random.Range(0, instance.m_Grid.m_GridWidth);
        int randY = Random.Range(0, instance.m_Grid.m_GridHeight - 1) + 1;

        instance.m_Grid.m_Nodes[randX, randY].m_RespawnType = prefabToSpawn;
        instance.m_Grid.m_Nodes[randX, randY].m_RespawnIsSpawned = false;
        instance.m_Grid.m_Nodes[randX, randY].StartDestroy(false);
    }
}