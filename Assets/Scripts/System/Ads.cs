﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;

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

    public static bool AdAllowed = true;
    private static System.Action<ShowResult> onShowTurn;
    private static GameObject spawnPrefab;

    public static void Show(System.Action<ShowResult> callback)
    {
        if (!CanShowAd())
            return;

        ShowOptions options = new ShowOptions();
        options.resultCallback = callback;

        Advertisement.Show("rewardedVideo", options);
    }

    public static bool CanShowAd()
    {
        return (!Advertisement.isShowing && Advertisement.IsReady("rewardedVideo")) && AdAllowed;
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

    public static void AdForRevive()
    {
        Show(ReviveAdShown);
    }

    public static void AdForHero(GameObject prefab)
    {
        spawnPrefab = prefab;
        Show(HeroAdShown);
    }

    public static void ReviveAdShown(ShowResult result)
    {
        //Revive if success
        if (result == ShowResult.Finished)
        {
            Analytics.CustomEvent("Revived");

            AdAllowed = false;

            GameManager.instance.Score = Mathf.Max(GameManager.instance.Score, (int)(Mediator.Settings.TargetScore * 0.75f));

            GameManager.instance.m_TurnsLeft = Mediator.Settings.Turns / 2;
            GameManager.instance.m_TurnsText.text = (5).ToString();
            GameManager.instance.m_LosePanel.SetActive(false);
            GameManager.instance.m_IsGameOver = false;

            if (GameManager.instance.m_TotalGameOver)
            {
                GameManager.instance.m_Grid.ResetBoard();
                GameManager.instance.m_TotalGameOver = false;
            }
        }
    }

    public static void HeroAdShown(ShowResult result)
    {
        if(result == ShowResult.Finished)
        {
            Analytics.CustomEvent("Hero Ad Spawned");

            //Replace a random tile with a hero
            GameManager.ReplaceRandomTile(spawnPrefab);
        }
    }
}