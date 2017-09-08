using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public float Percentage = 0.0f;

    GameObject HealthBarObject;
    GameObject StartPosition;
    GameObject FinishPosition;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Mathf.Lerp(StartPosition.


        //Clamping percentage 
        if (Percentage < 100)
        {
            Percentage = 100;
        }
        else if (Percentage > 0)
        {
            Percentage = 0;
        }


        



    }





}
