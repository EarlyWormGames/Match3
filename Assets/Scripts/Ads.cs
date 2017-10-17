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

    public static void AdForTurns()
    {
        AdManager.Show(TurnAdShown);
    }

    public static void TurnAdShown(ShowResult result)
    {
        //Give turn if success
    }

    public static void AdForItem(int a_item)
    {
        item = a_item;
        AdManager.Show(ItemAdShown);
    }

    public static void ItemAdShown(ShowResult result)
    {
        //Give item if success
    }

    public static void AdForRevive()
    {
        AdManager.Show(ReviveAdShown);
    }

    public static void ReviveAdShown(ShowResult result)
    {
        //Revive if success

        if (result == ShowResult.Finished)
        {
            if (Mediator.Settings != null)
                GameManager.Score = (int)(Mediator.Settings.TargetScore * 0.75f);

            GameManager.instance.m_TurnsLeft = 5;
        }
    }
}