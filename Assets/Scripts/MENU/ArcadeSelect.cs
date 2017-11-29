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

    // Use this for initialization
    void Start()
    {
        LevelNum = SaveData.LastArcade + 1;

        levelDisplay.text = (LevelNum + 1).ToString();
        scoreDisplay.text = SaveData.ArcadeScore.ToString();
    }

    protected override void OnClick()
    {
        selected = this;
        sceneMan.ChangeSceneToAndSetUpMediator(scene);
    }
}
