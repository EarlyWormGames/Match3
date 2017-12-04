using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuyUI : MonoBehaviour
{
    public delegate void FinishDel();
    public static BadGuyUI instance;

    public Animator m_Animator;
    public FinishDel m_ShowDone;
    public FinishDel m_HideDone;
    public FinishDel m_BlockedDone;
    public bool UseInstance = true;

    // Use this for initialization
    void Start()
    {
        if (UseInstance)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        m_Animator.SetBool("Hide", false);
        m_Animator.SetBool("Blocked", false);
    }

    public void Blocked()
    {
        m_Animator.SetBool("Blocked", true);
    }

    public void Hide()
    {
        m_Animator.SetBool("Hide", true);
    }

    public void ShowDone()
    {
        if (m_ShowDone != null)
            m_ShowDone();
    }

    public void BlockedDone()
    {
        if (m_BlockedDone != null)
            m_BlockedDone();
    }

    public void HideDone()
    {
        if (m_HideDone != null)
            m_HideDone();
    }
}