using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BadGuyUI : MonoBehaviour
{
    public delegate void FinishDel();
    public static BadGuyUI instance;

    public Animator m_Animator;
    public FinishDel m_ShowDone;
    public FinishDel m_HideDone;
    public FinishDel m_BlockedDone;
    public FinishDel m_ShrinkDone;
    public bool UseInstance = true;

    [Tooltip("List of things this character will say. Can include rich text tags")]
    public string[] OneLiners;
    public TextMeshProUGUI Text;

    private bool shrunk;

    // Use this for initialization
    void Start()
    {
        if (UseInstance)
            instance = this;

        if (Text != null)
        {
            int rand = Random.Range(0, OneLiners.Length);
            Text.text = OneLiners[rand];
        }
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
        m_Animator.SetTrigger(shrunk ? "HideSmall" : "Hide");
    }

    public void Shrink()
    {
        shrunk = true;
        m_Animator.SetTrigger("Shrink");
    }

    public void UnShrink()
    {
        shrunk = false;
        m_Animator.SetTrigger("UnShrink");
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

    public void ShrinkDone()
    {
        if (m_ShrinkDone != null)
            m_ShrinkDone();
    }
}