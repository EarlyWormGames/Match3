using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Facebook.Unity;
using TMPro;

public class FBHighscores : MonoBehaviour
{
    [Header("Login")]
    public GameObject LoginButton;
    public GameObject LogoutButton;
    public TextMeshProUGUI LogoutButtonName;

    [Header("User Info")]
    public RawImage ProfileImage;
    public TextMeshProUGUI ProfileName;

    public Transform ScoresParent;
    public GameObject PersonPrefab;

    public UnityEvent OnLogIn, OnLogOut;

    private Dictionary<string, GameObject> scores = new Dictionary<string, GameObject>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AppStart()
    {
        FB.Init();
    }

    private void OnEnable()
    {
        GameStateManager.OnRedrawUI += Redraw;
        GameStateManager.OnFriendUpdate += UpdateFriendImages;
    }

    private void OnDisable()
    {
        GameStateManager.OnRedrawUI -= Redraw;
        GameStateManager.OnFriendUpdate -= UpdateFriendImages;
    }

    // Use this for initialization
    void Start()
    {
        if (FB.IsLoggedIn)
        {
            OnLogIn.Invoke();
            FBGraph.GetPlayerInfo();
            FBGraph.GetFriends();
            FBGraph.GetScores();
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

    public void Redraw(UiUpdateEvent eventType)
    {
        switch (eventType)
        {
            case UiUpdateEvent.PlayerInfo:
                UpdatePlayerInfo();
                break;

            case UiUpdateEvent.Scores:
                UpdateScores();
                break;
        }
    }

    void UpdatePlayerInfo()
    {
        LogoutButtonName.text = "Logged in as " + GameStateManager.Username.Split(' ')[0];

        ProfileName.text = GameStateManager.Username;
        ProfileImage.texture = GameStateManager.UserTexture;
    }

    void UpdateScores()
    {
        foreach (var item in scores)
        {
            Destroy(item.Value);
        }
        scores.Clear();

        foreach (var item in GameStateManager.Scores)
        {
            var entry = (Dictionary<string, object>)item;
            var user = (Dictionary<string, object>)entry["user"];
            string id = (string)user["id"];
            int score = GraphUtil.GetScoreFromEntry(entry);

            GameObject person = Instantiate(PersonPrefab, ScoresParent);
            person.GetComponentInChildren<TextMeshProUGUI>().text = score.ToString();

            if(GameStateManager.FriendImages.ContainsKey(id))
                person.GetComponentInChildren<RawImage>().texture = GameStateManager.FriendImages[id];

            scores.Add(id, person);
        }
    }

    void UpdateFriendImages(string id)
    {
        if (!scores.ContainsKey(id))
            return;

        scores[id].GetComponentInChildren<RawImage>().texture = GameStateManager.FriendImages[id];
    }

    public static void SetScore(int score)
    {
        var scoreData = new Dictionary<string, string>() { { "score", score.ToString() } };
        FB.API("/me/scores", HttpMethod.POST, SetScoreCallback, scoreData);
    }

    static void SetScoreCallback(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }

        Debug.Log("Score updated successfully");
    }
}