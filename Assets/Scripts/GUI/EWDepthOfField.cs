using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class EWDepthOfField : MonoBehaviour {

    

    float PauseFocusDistance = 0.1f;
    float GameFocusDistance = 1.0f;

    bool FocusOnGame = true;

    bool blah;
    
    DepthOfFieldModel.Settings RunTimeSettings;

    PostProcessingProfile profile;
    DepthOfFieldModel Depth;
    
    //for setting back the asset file back to its orional state.
    DepthOfFieldModel.Settings StartSettings;

	// Use this for initialization
	void Start () {
        profile = Camera.main.GetComponent<PostProcessingBehaviour>().profile;
        Depth = profile.depthOfField;

        StartSettings = Depth.settings;


    }
	
	// Update is called once per frame
	void Update ()
    {

        
        if (FocusOnGame)
        {

        }
        else
        {

        }







        Depth.settings = RunTimeSettings;
    }



    void SetFocus(float a_focus)
    {

    }

    private void OnApplicationQuit()
    {
        Depth.settings = StartSettings;
    }

    public void FocusOnPauseGUI()
    {
        SetFocus(1);
    }

    public void FocusOnGame()
    {
        SetFocus(1);
    }





}
