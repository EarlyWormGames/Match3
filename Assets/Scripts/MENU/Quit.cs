using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {

    Fading fade;

	// Use this for initialization
	void Start () {
        fade = GetComponent<Fading>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}



    public void QuitGame()
    {
        fade.BeginFade(1);
    }
}
