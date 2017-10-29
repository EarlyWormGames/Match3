using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class MenuAds : MonoBehaviour
{
    public int LevelCount = 10;
    private static MenuAds instance;

    // Use this for initialization
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        SaveData.Load(LevelCount);
        DontDestroyOnLoad(gameObject);
    }

    public void GetTurns()
    {
        AdManager.AdForTurns(TurnAdOver);
    }

    public void TurnAdOver(ShowResult result)
    {
        SaveData.FreeTurns += 3;
        SaveData.Save();
    }
}
