using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject m_DragPrefab;
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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
