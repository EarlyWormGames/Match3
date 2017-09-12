using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class EWDepthOfField : MonoBehaviour {

    PostProcessingProfile profile;
    DepthOfFieldModel Depth;


    DepthOfFieldModel.Settings RunTimeSettings;



    //for setting back the asset file back to its orional state.
    DepthOfFieldModel.Settings StartSettings;

	// Use this for initialization
	void Start () {
        profile = Camera.main.GetComponent<PostProcessingBehaviour>().profile;
        Depth = profile.depthOfField;

        StartSettings = Depth.settings;
    }
	
	// Update is called once per frame
	void Update () {
        






    }



    public void SetFocus(float a_focus)
    {

    }

    private void OnApplicationQuit()
    {
        Depth.settings = StartSettings;
    }






}
