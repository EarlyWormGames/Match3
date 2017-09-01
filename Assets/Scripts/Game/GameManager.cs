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
    internal static GameObject dragObject;
    internal static Vector3 dragStartPos;
    internal static int Score;


    internal static List<NodeItem> NodeChainLeft = new List<NodeItem>(),
        NodeChainRight = new List<NodeItem>(),
        NodeChainUp = new List<NodeItem>(),
        NodeChainDown = new List<NodeItem>();

    #endregion

    public Text m_ScoreText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_ScoreText.text = Score.ToString();
    }
}
