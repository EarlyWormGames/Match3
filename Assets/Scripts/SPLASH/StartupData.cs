using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class StartupData : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Analytics.CustomEvent("App Launched");
    }
}
