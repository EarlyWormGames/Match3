using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArcadeSelect : LevelSettings
{
    [System.Serializable]
    public class LevelSize
    {
        public int num;
        public Vector2 size;
    }

    public ChangeScene sceneMan;
    public string scene;
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI scoreDisplay;
    public float TurnsLog = 10;
    public LevelSize[] levelSizes;

    // Use this for initialization
    void Start()
    {
        LevelNum = SaveData.LastArcade + 1;

        levelDisplay.text = (LevelNum + 1).ToString();
        scoreDisplay.text = SaveData.ArcadeScore.ToString();

        TurnsGoal = (int)(Mathf.Log(LevelNum + 1, TurnsLog) + 0.5f) + 40;
        DifficultyMult = Mathf.Log(LevelNum + 1, TurnsLog);
        int i = 0;
        for (i = 0; i < levelSizes.Length; ++i)
        {
            if (levelSizes[i].num > LevelNum)
                break;
        }

        GridWidth = (int)levelSizes[i].size.x;
        GridHeight = (int)levelSizes[i].size.y;
    }

    protected override void OnClick()
    {
        selected = this;
        sceneMan.ChangeSceneToAndSetUpMediator(scene);
    }
}
