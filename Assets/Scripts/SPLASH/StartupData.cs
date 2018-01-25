using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Analytics;

public class StartupData : MonoBehaviour
{
    public AudioMixerGroup Master;

    // Use this for initialization
    void Start()
    {
        Analytics.CustomEvent("App Launched");

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            Master.audioMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        }
        if (PlayerPrefs.HasKey("SfxVol"))
        {
            Master.audioMixer.SetFloat("SfxVol", PlayerPrefs.GetFloat("SfxVol"));
        }

        if (PlayerPrefs.HasKey("MusicMute"))
        {
            Master.audioMixer.SetFloat("MusicMute", PlayerPrefs.GetFloat("MusicMute"));
        }
        if (PlayerPrefs.HasKey("SfxMute"))
        {
            Master.audioMixer.SetFloat("SfxMute", PlayerPrefs.GetFloat("SfxMute"));
        }
    }
}
