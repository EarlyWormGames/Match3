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
            float LerpX = ObjectToMove.transform.position.x;
            float LerpY = ObjectToMove.transform.position.y;
            float LerpZ = ObjectToMove.transform.position.z;

            //Clamping percentage 
            if (Percentage > 100)
            {
                Percentage = 100;
            }
            else if (Percentage < 0)
            {
                Percentage = 0;
            }

            // 1/100 = 1% also 0.01
            float DecimalPercent = Percentage / 100;

            LerpX = Mathf.Lerp(StartPosition.transform.position.x, FinishPosition.transform.position.x, DecimalPercent);
            LerpY = Mathf.Lerp(StartPosition.transform.position.y, FinishPosition.transform.position.y, DecimalPercent);
            LerpZ = Mathf.Lerp(StartPosition.transform.position.z, FinishPosition.transform.position.z, DecimalPercent);

            //Movement of the Object
            ObjectToMove.transform.position = new Vector3(LerpX, LerpY, LerpZ);
        }
    }
}
