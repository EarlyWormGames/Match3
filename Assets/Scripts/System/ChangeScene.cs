using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Analytics;

//Dude if it needs a fading script, just put this in here ffs
[RequireComponent(typeof(Fading))]
public class ChangeScene : MonoBehaviour
{
    public bool isOpeningScene = false;
    public string LevelSelect = "WorldSelection", ArcadeScene = "Arcade";
    public UnityEvent OnWillChangeScene;

    Fading fade;

    // Use this for initialization
    void Start()
    {
        fade = GetComponent<Fading>();

        if(isOpeningScene)
        {
            if (PlayerPrefs.GetInt("OpenedGame") > 0)
                ChangeSceneTo("Menu");
            else
                PlayerPrefs.SetInt("OpenedGame", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

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
            OnWillChangeScene.Invoke();
        }
    }

    public void ReturnFromGame()
    {
        Analytics.CustomEvent("Return To Menu", new Dictionary<string, object>
            {
                { "level", Mediator.Settings.Level }
            });

        ChangeSceneTo(Mediator.Settings.isArcade? ArcadeScene : LevelSelect);
        OnWillChangeScene.Invoke();
    }

    public void ChangeSceneToAndSetUpMediator(string TargetScene)
    {
        if (TargetScene == "")
        {
            Debug.LogError("You DUN goofed - change the inspector TargetScene to a scene that exsists in the build settings.");
        }
        else
        {
            SetupMediator();
            fade.BeginFade(1, TargetScene);
            OnWillChangeScene.Invoke();
        }
    }

    void SetupMediator()
    {
        LevelSettings LS = LevelSettings.selected;

        if (LS != null)
        {
            if (Mediator.Settings == null)
                Mediator.Settings = new GameSettings();

            Mediator.Settings.RequiredChain = LS.RequiredChain;
            Mediator.Settings.ColourChance = LS.ColourChance;
            Mediator.Settings.GridWidth = LS.GridWidth;
            Mediator.Settings.GridHeight = LS.GridHeight + 1;
            Mediator.Settings.TargetScore = LS.TargetScore;
            Mediator.Settings.DifficultyMult = LS.DifficultyMult;
            Mediator.Settings.Turns = LS.TurnsGoal;
            Mediator.Settings.Level = LS.LevelNum;
            Mediator.Settings.isArcade = LS.isArcade;
            Mediator.Settings.StartNodes = LS.JoinRows();

            if (!LS.isArcade)
            {
                Analytics.CustomEvent("Game Started", new Dictionary<string, object>
                {
                    { "level", Mediator.Settings.Level }
                });
            }
            else
            {
                Analytics.CustomEvent("Arcade Started", new Dictionary<string, object>
                {
                    { "level", Mediator.Settings.Level }
                });
            }
        }
        else
        {
            Debug.LogError("Could not find the current selected level.");
        }
    }
}