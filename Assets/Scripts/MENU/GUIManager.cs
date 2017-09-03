using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GUIManager : MonoBehaviour {

    bool m_bOpenScore = false;
    public Animator[] MainAnimControllers;
    public Animator ScorePanel;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
    }

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

}
