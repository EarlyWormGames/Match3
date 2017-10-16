using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ChangeScene : MonoBehaviour {
    
    Fading fade;

    // Use this for initialization
    void Start () {
        fade = GetComponent<Fading>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeSceneTo(string TargetScene)
    {
        SetupMediator();
        
        if (TargetScene == "")
        {
            Debug.LogError("You DUN goofed - change the inspector TargetScene to a scene that exsists in the build settings.");
        }
        else
        {
            fade.BeginFade(1, TargetScene);
        }
    }

    void SetupMediator()
    {
        GameObject SelectedLevel = GameObject.Find(EventSystem.current.currentSelectedGameObject.name);

        if (SelectedLevel)
        {
            LevelSettings LS = SelectedLevel.GetComponent<LevelSettings>();
            if(Mediator.Settings == null)
                Mediator.Settings = new GameSettings();

            Mediator.Settings.RequiredChain = LS.RequiredChain;
            Mediator.Settings.ColourChance = LS.ColourChance;
            Mediator.Settings.GridWidth = LS.GridWidth;
            Mediator.Settings.GridHeight = LS.GridHeight + 1;
            Mediator.Settings.TargetScore = LS.TargetScore;
            Mediator.Settings.DifficultyMult = LS.DifficultyMult;
        }
        else
        {
            Debug.LogError("Could not find the current selected level.");
        }


    }

}
