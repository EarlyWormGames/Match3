﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Button m_ReviveButton;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_ReviveButton.interactable = AdManager.CanShowAd();
    }

    public void MoreTurns()
    {
        AdManager.AdForRevive();
    }

    public void Menu()
    {
        GameManager.instance.SubmitScore("WorldSelection");
    }

    public void MainMenu()
    {
        GameManager.instance.SubmitScore("Menu");
    }

    public void Arcade()
    {
        GameManager.instance.SubmitScore("Arcade");
    }

    public void TryAgain()
    {
        GameManager.instance.SubmitScore("Game");
    }
}
