using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public Button UI_Music;
    public Button UI_SFX;
    public Slider MusicSlider, SfxSlider;

    public GameObject ConfirmationPanel;

    public AudioMixerGroup Master;

    bool m_bMuteMusic = false;
    bool m_bMuteSFX = false;

    // Use this for initialization
    void Start()
    {
        float val = 0;
        Master.audioMixer.GetFloat("MusicMute", out val);
        if (val < 0)
            MuteMusic();

        Master.audioMixer.GetFloat("SfxMute", out val);
        if (val < 0)
            MuteSFX();

        Master.audioMixer.GetFloat("MusicVol", out val);
        MusicSlider.value = val;

        Master.audioMixer.GetFloat("SfxVol", out val);
        SfxSlider.value = val;
    }

    void UnMuteMusic()
    {
        UI_Music.GetComponentInChildren<TextMeshProUGUI>().text = "Music:\nMUTE?";
        m_bMuteMusic = false;
    }
    void MuteMusic()
    {
        UI_Music.GetComponentInChildren<TextMeshProUGUI>().text = "Music:\nMUTED";
        m_bMuteMusic = true;
    }

    void UnMuteSFX()
    {
        UI_SFX.GetComponentInChildren<TextMeshProUGUI>().text = "SFX:\nMUTE?";
        m_bMuteSFX = false;
    }
    void MuteSFX()
    {
        UI_SFX.GetComponentInChildren<TextMeshProUGUI>().text = "SFX:\nMUTED";
        m_bMuteSFX = true;
    }

    public void ResetScores()
    {
        ConfirmationPanel.SetActive(true);
    }

    public void ToggleMusic()
    {
        if (!m_bMuteMusic)
        {
            MuteMusic();
        }
        else
        {
            UnMuteMusic();
        }
        MuteItem("MusicMute");
    }

    public void ToggleSFX()
    {
        if (!m_bMuteSFX)
        {
            MuteSFX();
        }
        else
        {
            UnMuteSFX();
        }
        MuteItem("SfxMute");
    }

    public void ConfirmationYES()
    {
        ConfirmationPanel.SetActive(false);

        if (File.Exists(Application.persistentDataPath + "/" + "highscores.txt"))
            File.Delete(Application.persistentDataPath + "/" + "highscores.txt");
        if (File.Exists(Application.persistentDataPath + "/" + "save.data"))
            File.Delete(Application.persistentDataPath + "/" + "save.data");

        FindObjectOfType<HighScores>().LoadScoresFromFile();
        SaveData.Load();
    }
    public void ConfirmationNO()
    {
        ConfirmationPanel.SetActive(false);
    }

    public void MuteItem(string group)
    {
        float currentval = 0;
        if (PlayerPrefs.HasKey(group))
        {
            currentval = PlayerPrefs.GetFloat(group);
        }

        if (currentval < 0)
        {
            //Toggle sound on
            Master.audioMixer.SetFloat(group, 0);
            PlayerPrefs.SetFloat(group, 0);
        }
        else
        {
            //Toggle sound off
            Master.audioMixer.SetFloat(group, -80);
            PlayerPrefs.SetFloat(group, -80);
        }
        PlayerPrefs.Save();
    }

    public void SetMusicVol(float value)
    {
        Master.audioMixer.SetFloat("MusicVol", value);
        PlayerPrefs.SetFloat("MusicVol", value);
        PlayerPrefs.Save();
    }

    public void SetSfxVol(float value)
    {
        Master.audioMixer.SetFloat("SfxVol", value);
        PlayerPrefs.SetFloat("SfxVol", value);
        PlayerPrefs.Save();
    }
}