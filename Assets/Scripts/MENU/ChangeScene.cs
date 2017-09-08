using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    
    Fading fade;

    // Use this for initialization
    void Start () {
        fade = GetComponent<Fading>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeSceneTo(string TargetScene)
    {
        if (TargetScene == "")
        {
            Debug.LogError("You DUN goofed - change the inspector TargetScene to a scene that exsists in the build settings.");
        }
        else
        {
            fade.BeginFade(1, TargetScene);
        }
    }
}
