using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class EWDepthOfField : MonoBehaviour {

    public bool m_bfocus = true;
    public float BlurSpeed = 1.0f;
    //focus levels settings
    float PauseFocusDistance = 0.1f;
    float GameFocusDistance = 1.0f;
    //settings during the game
    DepthOfFieldModel.Settings RunTimeSettings;
    //Game Profile and DoF settings
    PostProcessingProfile profile;
    DepthOfFieldModel Depth;
    //for setting back the asset file back to its orional state.
    DepthOfFieldModel.Settings StartSettings;

	// Use this for initialization
	void Start () {
        profile = Camera.main.GetComponent<PostProcessingBehaviour>().profile;
        Depth = profile.depthOfField;

        StartSettings = Depth.settings;
        RunTimeSettings = StartSettings;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_bfocus)
        {
            //Unpaused
            if (RunTimeSettings.focusDistance < GameFocusDistance)
            {
                RunTimeSettings.focusDistance += BlurSpeed * Time.deltaTime;
            }
        }
        else
        {
            //Pause
            if (RunTimeSettings.focusDistance > PauseFocusDistance)
            {
                RunTimeSettings.focusDistance -= BlurSpeed * Time.deltaTime;
            }   
        }
        //Assign settings to the post prosessing settings
        Depth.settings = RunTimeSettings;
    }

    public void ToggleFocus()
    {
        m_bfocus = !m_bfocus;
    }

    private void OnApplicationQuit()
    {
        Depth.settings = StartSettings;
    }
}
