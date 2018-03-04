using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Facebook.Unity;
using TMPro;

public class FBHighscores : MonoBehaviour
{
    public GameObject LoginButton;
    public GameObject LogoutButton;
    public TextMeshProUGUI LogoutButtonName;
    public RawImage ProfileImage;
    public TextMeshProUGUI ProfileName;

    public UnityEvent OnLogIn, OnLogOut;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AppStart()
    {
        FB.Init();
    }

    private void OnEnable()
    {
        GameStateManager.OnRedrawUI += Redraw;
    }

    private void OnDisable()
    {
        GameStateManager.OnRedrawUI -= Redraw;
    }

    // Use this for initialization
    void Start()
    {
        if (FB.IsLoggedIn)
        {
            OnLogIn.Invoke();
            FBGraph.GetFriends();
            FBGraph.GetScores();

            LogoutButtonName.text = "Logged in as " + GameStateManager.Username.Split(' ')[0];
        }
        else
        {
            LogoutButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Login()
    {
        FBLogin.PromptForPublish(LoggedIn);
    }

    void LoggedIn()
    {
        if (FB.IsLoggedIn)
        {
            OnLogIn.Invoke();
            FBGraph.GetPlayerInfo();
            FBGraph.GetFriends();
            FBGraph.GetScores();

            //Debug.Log(GameStateManager.Scores.Count);
            //foreach (Dictionary<string, object> item in GameStateManager.Scores)
            //{
            //    var user = (Dictionary<string, object>)item["user"];
            //    Debug.Log(item["score"]);
            //    ProfileImage.texture = GameStateManager.FriendImages[(string)user["id"]];
            //}
        }
    }

    public void Logout()
    {
        FB.LogOut();

        OnLogOut.Invoke();
    }

    public void Redraw()
    {
        if (!string.IsNullOrEmpty(GameStateManager.Username))
        {
            LogoutButtonName.text = "Logged in as " + GameStateManager.Username.Split(' ')[0];

            ProfileName.text = GameStateManager.Username;
            ProfileImage.texture = GameStateManager.UserTexture;
        }
    }
}