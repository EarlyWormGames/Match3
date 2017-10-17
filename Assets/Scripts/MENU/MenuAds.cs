using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class MenuAds : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SavedData.Load();
    }

    public void GetTurns()
    {
        AdManager.AdForTurns(TurnAdOver);
    }

    public void TurnAdOver(ShowResult result)
    {
        SavedData.FreeTurns += 3;
        SavedData.Save();
    }
}
