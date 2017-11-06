using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsCanvasManager : MonoBehaviour
{

    bool m_bOpen = false;
    public Animator OptionsAnim;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    public void OpenSettingsMenu()
    {
        m_bOpen = true;
        OptionsAnim.SetBool("Enter", true);
        OptionsAnim.SetTrigger("Triger");
        GameManager.CanDrag = false;
    }

    public void CloseSettingsMenu()
    {
        m_bOpen = false;
        OptionsAnim.SetBool("Enter", false);
        OptionsAnim.SetTrigger("Triger");
        GameManager.CanDrag = true;
    }


}
