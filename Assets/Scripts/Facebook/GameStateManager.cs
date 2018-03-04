using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    public static string Username;
    public static Texture UserTexture;
    public static int Score, HighScore;
    public static Dictionary<string, Texture> FriendImages = new Dictionary<string, Texture>();
    public static List<object> Scores = new List<object>();
    public static List<object> Friends = new List<object>();
    public static string FriendID;
    public static string ServerURL;

    public static System.Action OnRedrawUI;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void CallUIRedraw()
    {
        if (OnRedrawUI != null)
            OnRedrawUI.Invoke();
    }

    public static void ShowPopup(string text, UnityAction action = null)
    {

    }
}