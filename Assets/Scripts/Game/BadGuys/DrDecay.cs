using System.Collections;
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DamageScore()
    {
        if (hideOnShow)
        {
            m_UIObject.Hide();
            return;
        }

        int sub = (int)(Mediator.Settings.TargetScore * (DamageMult * (Mediator.Settings.DifficultyMult / 2f)));
        GameManager.Score = Mathf.Clamp(GameManager.Score - sub, 0, Mediator.Settings.TargetScore);
    }

    void ShowDone()
    {
        DamageScore();
        StartCoroutine(WaitTimer());
    }

    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(m_WaitSeconds);
        m_UIObject.Hide();
    }

    void HideDone()
    {
        Destroy(m_UIObject.gameObject);
        Destroy(gameObject);
    }
}
