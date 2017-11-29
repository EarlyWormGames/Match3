using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArcadeSelect : LevelSettings
{
    public ChangeScene sceneMan;
    public string scene;
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI scoreDisplay;
    public float TurnsLog = 10;

    // Use this for initialization
    void Start()
    {
        LevelNum = SaveData.LastArcade + 1;

        levelDisplay.text = (LevelNum + 1).ToString();
        scoreDisplay.text = SaveData.ArcadeScore.ToString();

        TurnsGoal = (int)(Mathf.Log(LevelNum + 1, TurnsLog) + 0.5f) + 40;
    }

    protected override void OnClick()
    {
        selected = this;
        sceneMan.ChangeSceneToAndSetUpMediator(scene);
    }
}
