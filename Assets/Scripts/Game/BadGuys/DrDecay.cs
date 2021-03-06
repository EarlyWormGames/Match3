﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrDecay : BadGuy
{
    public float DamageMult = 0.2f;
    public float m_WaitSeconds = 1;
    public GameObject m_UIPrefab;
    private DrDecayUI m_UIObject;
    private bool hideOnShow;


    public override void NoEffect()
    {
        hideOnShow = true;
    }

    // Use this for initialization
    void Start()
    {
        m_UIObject = Instantiate(m_UIPrefab).GetComponent<DrDecayUI>();
        m_UIObject.transform.SetParent(GameManager.instance.m_MainCanvas.transform, false);

        if (hideOnShow)
        {
            Destroy(m_UIObject.m_ZapParticle.gameObject);
            Destroy(m_UIObject.transform.GetChild(0).gameObject);
        }

        //Assign some delegates for the UI animating
        m_UIObject.m_ShowDone += ShowDone;
        m_UIObject.m_HideDone += HideDone;
        CharacterShower.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            if (Input.GetMouseButtonUp(0) && !GameManager.instance.trueUIBlocked)
            {
                waiting = false;
                m_UIObject.Hide();
            }
        }
    }

    void DamageScore()
    {
        if (hideOnShow)
        {
            m_UIObject.Hide();
            return;
        }

        int sub = (int)(Mediator.Settings.TargetScore * (DamageMult * (Mediator.Settings.DifficultyMult / 2f)));
        GameManager.instance.Score = Mathf.Clamp(GameManager.instance.Score - sub, 0, Mediator.Settings.TargetScore);
    }

    void ShowDone()
    {
        DamageScore();
        m_UIObject.EndSlide();
        waiting = true;
    }

    void HideDone()
    {
        Destroy(m_UIObject.gameObject);
        Destroy(gameObject);
        CharacterShower.FadeOut();
    }
}
