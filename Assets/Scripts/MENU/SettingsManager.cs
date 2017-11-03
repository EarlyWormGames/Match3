using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {


    public Button UI_Music;
    public Button UI_SFX;
    public GameObject ConfirmationPanel;

    bool m_bMuteMusic = false;
    bool m_bMuteSFX = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UnMuteMusic()
    {
        UI_Music.GetComponentInChildren<Text>().text = "Music:\nMUTE?";
    }
    void MuteMusic()
    {
        UI_Music.GetComponentInChildren<Text>().text = "Music:\nMUTED";
    }

    void UnMuteSFX()
    {
        UI_SFX.GetComponentInChildren<Text>().text = "SFX:\nMUTE?";
    }
    void MuteSFX()
    {
        UI_SFX.GetComponentInChildren<Text>().text = "SFX:\nMUTED";
    }

    public void ResetScores()
    {
        ConfirmationPanel.SetActive(true);
    }

    public void ToggleMusic()
    {
        m_bMuteMusic = !m_bMuteMusic;

        if (m_bMuteMusic)
        {
            MuteMusic();
        }
        else
        {
            UnMuteMusic();
        }
    }

    public void ToggleSFX()
    {
        m_bMuteSFX = !m_bMuteSFX;
        if (m_bMuteSFX)
        {
            MuteSFX();
        }
        else
        {
            UnMuteSFX();
        }
    }

    public void ConfirmationYES()
    {
        ConfirmationPanel.SetActive(false);

        if (File.Exists(Application.persistentDataPath + "\\" + "highscores.txt"))
            File.Delete(Application.persistentDataPath + "\\" + "highscores.txt");
        if (File.Exists(Application.persistentDataPath + "\\" + "SaveData.txt"))
            File.Delete(Application.persistentDataPath + "\\" + "SaveData.txt");

        FindObjectOfType<HighScores>().LoadScoresFromFile();
        SaveData.Load(SaveData.LevelCount);
    }
    public void ConfirmationNO()
    {
        ConfirmationPanel.SetActive(false);
    }


}
