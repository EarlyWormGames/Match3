using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GUIManager : MonoBehaviour {

   
    //All Menu Buttons animators
    public Animator[] MainAnimControllers;

    //Panel Animators
    public Animator ScorePanel;
    public Animator SettingsPanel;

    //Helper variables for the pannel status
    bool m_bOpenScore = false;
    bool m_bOpenSettings = false;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {   
    }

    // Score Panel
    //==================================
    void OpenScore()
    {
        ScorePanel.SetBool("Enter", true);
        ScorePanel.SetTrigger("Trigger");
    }
    void CloseScore()
    {
        ScorePanel.SetBool("Enter", false);
        ScorePanel.SetTrigger("Trigger");
    }
    //==================================

    //All the Menu Buttons
    //==================================
    void OpenMenu()
    {
        for (int i = 0; i < MainAnimControllers.Length; i++)
        {
            MainAnimControllers[i].SetBool("Enter", true);
            MainAnimControllers[i].SetTrigger("Trigger");
        }
    }
    void CloseMenu()
    {
        for (int i = 0; i < MainAnimControllers.Length; i++)
        {
            MainAnimControllers[i].SetBool("Enter", false);
            MainAnimControllers[i].SetTrigger("Trigger");
        }
    }
    //==================================

    //Settings Panel
    //==================================
    void OpenSettings()
    {
        SettingsPanel.SetBool("Enter", true);
        SettingsPanel.SetTrigger("Trigger");
    }
    void CloseSettings()
    {
        SettingsPanel.SetBool("Enter", false);
        SettingsPanel.SetTrigger("Trigger");
    }
    //==================================



    //Set the status of the score panel  Open/Closed in the inspector
    public void SetScorePanel(bool b)
    {
        m_bOpenScore = b;

        if (m_bOpenScore)
        {
            OpenScore();
            CloseMenu();
        }
        else
        {
            OpenMenu();
            CloseScore();
        }
    }

    //Set the status of the Settings panel  Open/Closed in the inspector
    public void SetSettingsPanel(bool b)
    {
        m_bOpenSettings = b;

        if (m_bOpenSettings)
        {
            OpenSettings();
            CloseMenu();
        }
        else
        {
            OpenMenu();
            CloseSettings();
        }
    }
}
