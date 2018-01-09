using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsCanvasManager : MonoBehaviour
{
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
        OptionsAnim.SetBool("Enter", true);
        OptionsAnim.SetTrigger("Triger");
        GameManager.CanDrag = false;
    }

    public void CloseSettingsMenu()
    {
        OptionsAnim.SetBool("Enter", false);
        OptionsAnim.SetTrigger("Triger");
        GameManager.CanDrag = true;
    }


}
