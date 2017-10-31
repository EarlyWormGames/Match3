using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCanvasManager : MonoBehaviour {

    bool m_bOpen = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    


    public void OpenSettingsMenu()
    {
        m_bOpen = true;
    }

    public void ClsoeSettingsMenu()
    {
        m_bOpen = false;
    }
}
