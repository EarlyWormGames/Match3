using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DJHeartbeat : MonoBehaviour
{
    public float m_WaitSeconds = 1;
    public GameObject m_UIPrefab;
    private DJUI m_UIObject;
    private bool waitclick = false;

    // Use this for initialization
    void Start()
    {
        m_UIObject = Instantiate(m_UIPrefab).GetComponent<DJUI>();
        m_UIObject.transform.SetParent(GameManager.instance.m_MainCanvas.transform, false);

        //Assign some delegates for the UI animating
        m_UIObject.m_ShowDone += ShowDone;
        m_UIObject.m_HideDone += HideDone;

        CharacterShower.FadeIn();
        m_UIObject.Show();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitclick)
        {
            if (Input.GetMouseButtonDown(0) && !GameManager.instance.trueUIBlocked)
            {
                Reshuffle();
                m_UIObject.Hide();
            }
        }
    }

    void Reshuffle()
    {
        GameManager.instance.m_Grid.ResetBoard();
    }

    void ShowDone()
    {
        waitclick = true;
    }

    void HideDone()
    {
        CharacterShower.FadeOut();
        Destroy(m_UIObject.gameObject);
        Destroy(gameObject);
    }
}
