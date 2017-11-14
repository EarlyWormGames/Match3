using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleInEditorOnly : MonoBehaviour {

    private void Awake()
    {
        if (Debug.isDebugBuild)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
