using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Transition : MonoBehaviour {

    public float TrasitionAfter = 1.0f;
    float Timer = 0.0f;
    bool StartTransition = false;
    Fading fade;

    bool DoOnce = true;

	// Use this for initialization
	void Start () {
        //ensure the timer is set to 0
        Timer = TrasitionAfter;
        // Get the fading component for use in this script
        fade = GetComponent<Fading>();

    }
	
	// Update is called once per frame
	void Update () {

        // Only Count down if we havent started the fade transition
        if (StartTransition == false)
            Timer = Timer - Time.deltaTime; // Decrease the timer and count down to 0

        if (Timer < 0)
        {
            StartTransition = true;
        }

        if (StartTransition)
        {
            if (DoOnce)
            {
                fade.BeginFade(1, "Menu");
                DoOnce = false;
            }
        }


    }

    
}
