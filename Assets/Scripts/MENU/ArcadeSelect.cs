using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeSelect : LevelSettings
{
    public ChangeScene sceneMan;
    public string scene;

    // Use this for initialization
    void Start()
    {
        LevelNum = SaveData.LastArcade + 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnClick()
    {
        selected = this;
        sceneMan.ChangeSceneToAndSetUpMediator(scene);
    }
}
