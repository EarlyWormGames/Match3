using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class MenuAds : MonoBehaviour
{
    public static MenuAds instance;

    // Use this for initialization
    void Start()
    {

    }

    public void GetTurns()
    {
        AdManager.AdForTurns(TurnAdOver);
    }

    public void TurnAdOver(ShowResult result)
    {
        SaveData.instance.FreeTurns += 3;
        SaveData.Save();
    }
}
