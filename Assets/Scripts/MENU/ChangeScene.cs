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

    public void ChageToPlayGame()
    {
        fade.BeginFade(1);
        SceneManager.LoadScene("Game");
    }
}
