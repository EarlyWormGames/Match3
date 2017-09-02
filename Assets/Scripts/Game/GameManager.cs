﻿using System.Collections;
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
    internal static GameObject dragObject;
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
    private bool m_WasMoving = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_ScoreText.text = Score.ToString();

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

    public static void ClearNodeChains()
    {
        NodeChainLeft.Clear();
        NodeChainRight.Clear();
        NodeChainUp.Clear();
        NodeChainDown.Clear();
    }
}
