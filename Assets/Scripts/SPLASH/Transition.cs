using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Transition : MonoBehaviour {

    public float TrasitionAfter = 1.0f;
    float Timer;

	// Use this for initialization
	void Start () {
        Timer = TrasitionAfter;

    }
	
	// Update is called once per frame
	void Update () {
        Timer = Timer + Time.deltaTime;


        if (Timer < TrasitionAfter)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
