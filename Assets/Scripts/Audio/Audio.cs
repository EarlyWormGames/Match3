using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {

    public AudioSource MySource;
    bool paused = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
            {
                Time.timeScale = 0;
                MySource.Pause();
                paused = !paused;
            }
            else
            {
                Time.timeScale = 1;
                MySource.UnPause();
                paused = !paused;
            }
        }


    }
}
