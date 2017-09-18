using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuyUI : MonoBehaviour
{
    public delegate void FinishDel();

    public Animator m_Animator;
    public FinishDel m_ShowDone;
    public FinishDel m_HideDone;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {

    }

    public void Hide()
    {

    }

    public void ShowDone()
    {
        if (m_ShowDone != null)
            m_ShowDone();
    }

    public void HideDone()
    {
        if (m_HideDone != null)
            m_HideDone();
    }
}