using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;

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
        if (TargetScene == "")
        {
            Debug.LogError("You DUN goofed - change the inspector TargetScene to a scene that exsists in the build settings.");
        }
        else
        {
            fade.BeginFade(1, TargetScene);
        }
    }

    public void ReturnFromGame(string TargetScene)
    {
        Analytics.CustomEvent("Return To Menu", new Dictionary<string, object>
            {
                { "level", Mediator.Settings.Level }
            });

        ChangeSceneTo(TargetScene);
    }

    public void ChangeSceneToAndSetUpMediator(string TargetScene)
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
        LevelSettings LS = LevelSettings.selected;

        if (LS != null)
        {
            if(Mediator.Settings == null)
                Mediator.Settings = new GameSettings();

            Mediator.Settings.RequiredChain = LS.RequiredChain;
            Mediator.Settings.ColourChance = LS.ColourChance;
            Mediator.Settings.GridWidth = LS.GridWidth;
            Mediator.Settings.GridHeight = LS.GridHeight + 1;
            Mediator.Settings.TargetScore = LS.TargetScore;
            Mediator.Settings.DifficultyMult = LS.DifficultyMult;
            Mediator.Settings.Turns = LS.TurnsGoal;
            Mediator.Settings.Level = LS.transform.GetSiblingIndex();

            Analytics.CustomEvent("Game Started", new Dictionary<string, object>
                {
                    { "level", Mediator.Settings.Level }
                });
        }
        else
        {
            Debug.LogError("Could not find the current selected level.");
        }


    }

}
