using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsCanvasManager : MonoBehaviour
{
    public Animator OptionsAnim;

    private bool IsSettingsOpen = false;
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
        if (IsSettingsOpen == false)
        {
            OptionsAnim.SetBool("Enter", true);
            OptionsAnim.SetTrigger("Triger");
            GameManager.CanDrag = false;
            IsSettingsOpen = true;
        }
    }

    public void CloseSettingsMenu()
    {
        OptionsAnim.SetBool("Enter", false);
        OptionsAnim.SetTrigger("Triger");
        GameManager.CanDrag = true;
        IsSettingsOpen = false;
    }


}
