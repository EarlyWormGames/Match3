using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OpeningScene : MonoBehaviour
{
    [Tooltip("ONLY UPDATE THIS WHEN YOU WANT ALL USERS TO SEE THIS SCENE AGAIN")]
    public string LastChangedVersion = "1.4.5";
    public Text[] TextAnimations;
    public float AnimationSpeed = 1;
    public UnityEvent OnForceChange;

    private int currentTextIndex = -1;
    private float animTimer;
    private bool reverseTimer, finished, started;

    // Use this for initialization
    void Start()
    {
#if UNITY_EDITOR
        return; //Just skip this in the editor, since we can skip this manually anyway
#endif

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

    private void Update()
    {
        if(currentTextIndex >= 0)
        {
            animTimer = Mathf.Clamp(animTimer + Time.deltaTime, 0, AnimationSpeed);
            var col = TextAnimations[currentTextIndex].color;
            col.a = reverseTimer ? (1 - animTimer / AnimationSpeed) : (animTimer / AnimationSpeed);
            TextAnimations[currentTextIndex].color = col;

            if(animTimer >= AnimationSpeed && reverseTimer)
            {
                NextAnimation();
            }
        }
    }

    public void NextAnimation()
    {
        if(currentTextIndex > -1)
        {
            var col = TextAnimations[currentTextIndex].color;
            col.a = 0;
            TextAnimations[currentTextIndex].color = col;
        }

        started = true;
        reverseTimer = false;
        ++currentTextIndex;
        animTimer = 0;
        if(currentTextIndex >= TextAnimations.Length)
        {
            currentTextIndex = -1;
            finished = true;
            OnForceChange.Invoke();
            return;
        }
    }

    public void FadeAnimation()
    {
        if (currentTextIndex < 0)
            return;

        reverseTimer = true;
        animTimer = 0;
    }

    public void HandleClick()
    {
        if (finished || !started)
            return;

        if (reverseTimer)
            NextAnimation();
        else
            FadeAnimation();
    }
}