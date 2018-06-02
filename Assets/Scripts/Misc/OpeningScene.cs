using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OpeningScene : MonoBehaviour
{
    [Tooltip("ONLY UPDATE THIS WHEN YOU WANT ALL USERS TO SEE THIS SCENE AGAIN")]
    public string LastChangedVersion = "1.4.5";
    public UnityEvent OnForceChange; 

    // Use this for initialization
    void Start()
    {
        string version = PlayerPrefs.GetString("LastOpenedVersion");
        if (string.IsNullOrEmpty(version))
        {
            PlayerPrefs.SetString("LastOpenedVersion", LastChangedVersion);
            return;
        }

        string[] oldVersionSplit = version.Split('.');
        if (oldVersionSplit.Length != 3)
        {
            PlayerPrefs.SetString("LastOpenedVersion", LastChangedVersion);
            return;
        }

        string[] versionSplit = LastChangedVersion.Split('.');
        if (versionSplit.Length != 3)
        {
            PlayerPrefs.SetString("LastOpenedVersion", LastChangedVersion);
            return;
        }

        int oldmajor = Convert.ToInt32(oldVersionSplit[0]);
        int oldminor = Convert.ToInt32(oldVersionSplit[1]);
        int oldpatch = Convert.ToInt32(oldVersionSplit[2]);

        int major = Convert.ToInt32(versionSplit[0]);
        int minor = Convert.ToInt32(versionSplit[1]);
        int patch = Convert.ToInt32(versionSplit[2]);

        if (major > oldmajor || minor > oldminor || patch > oldpatch)
        {
            PlayerPrefs.SetString("LastOpenedVersion", LastChangedVersion);
            return;
        }

        //Current version is the same or newer, skip the scene
        OnForceChange.Invoke();
    }
}