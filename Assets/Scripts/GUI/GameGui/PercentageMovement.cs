using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageMovement : MonoBehaviour {

    public float Percentage = 0.0f;

    public GameObject StartPosition;
    public GameObject FinishPosition;

    GameObject ObjectToMove;
    bool NoErrors = true;

    // Use this for initialization
    void Start () {
        NoErrors = true;
        if (ObjectToMove == null)
        {
            ObjectToMove = this.gameObject;
        }

        if (StartPosition == null || FinishPosition == null)
        {
            NoErrors = false;
            Debug.LogError("Make Sure you attach your start and finish gameObjects");
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (NoErrors)
        {
            //Clamping percentage 
            if (Percentage > 1)
            {
                Percentage = 1;
            }
            else if (Percentage < 0)
            {
                Percentage = 0;
            }

            //Lerp the between the start and finish positions, using the Percentage
            Vector3 Lerp = Vector3.Lerp(StartPosition.transform.position, FinishPosition.transform.position, Percentage);

            //Movement of the Object
            ObjectToMove.transform.position = Lerp;
        }
    }
}
