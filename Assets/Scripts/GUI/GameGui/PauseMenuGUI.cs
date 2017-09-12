using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuGUI : MonoBehaviour
{
    public GameObject SettingsCog;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnGUI()
    {
        
    }


    public void OpenPauseMenu()
    {
        DisableSettingsCog();
        //PlayAnimation for pause gui
    }
    public void ClosePauseMenu()
    {
        EnableSettingsCog();
        //PlayAnimation for pause gui
    }


    //Helper Functions
    void EnableSettingsCog(){SettingsCog.GetComponent<Button>().interactable = true;}
    void DisableSettingsCog(){SettingsCog.GetComponent<Button>().interactable = false;}
}
