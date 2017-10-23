using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager
{
    // Use this for initialization
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        string id = "";
#if UNITY_ANDROID
        id = "1577515";
#elif UNITY_IOS
        id = "1577514";
#endif
        if (Advertisement.isSupported)
            Advertisement.Initialize(id, true);
    }

    private static System.Action<ShowResult> onShowTurn;

    public static void Show(System.Action<ShowResult> callback)
    {
        if (Advertisement.isShowing || !Advertisement.IsReady("rewardedVideo"))
            return;

        ShowOptions options = new ShowOptions();
        options.resultCallback = callback;

        Advertisement.Show("rewardedVideo", options);
    }

    public static bool CanShowAd()
    {
        return !(Advertisement.isShowing || !Advertisement.IsReady("rewardedVideo"));
    }

    public static int item;

    public static void AdForTurns(System.Action<ShowResult> callback)
    {
        Show(TurnAdShown);
        onShowTurn = callback;
    }

    public static void TurnAdShown(ShowResult result)
    {
        //Give turn if success

        onShowTurn(result);
    }

    public static void AdForItem(int a_item)
    {
        item = a_item;
        Show(ItemAdShown);
    }

    public static void ItemAdShown(ShowResult result)
    {
        //Give item if success
    }

    public static void AdForRevive()
    {
        Show(ReviveAdShown);
    }

    public static void ReviveAdShown(ShowResult result)
    {
        //Revive if success

        if (result == ShowResult.Finished)
        {
            GameManager.Score = Mathf.Max(GameManager.Score, (int)(Mediator.Settings.TargetScore * 0.75f));

            GameManager.instance.m_TurnsLeft = 5;
            GameManager.instance.m_TurnsText.text = (5).ToString();
            GameManager.instance.m_LosePanel.SetActive(false);
            GameManager.instance.m_IsGameOver = false;
        }
    }
}