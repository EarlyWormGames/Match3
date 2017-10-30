using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : MonoBehaviour
{
    public static PlayPanel instance;

    public StarShower StarPanel;
    public Text GameText;
    public Button PlayButton;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {

        if (LevelSettings.selected.transform.GetSiblingIndex() > 0)
        {
            if (SaveData.LevelScores[LevelSettings.selected.transform.GetSiblingIndex() - 1] <= 0)
            {
                ShowLockedLevel(LevelSettings.selected.transform.GetSiblingIndex() + 1);
                return;
            }
        }

        int score = SaveData.LevelScores[LevelSettings.selected.transform.GetSiblingIndex()];
        ShowLevelCard(LevelSettings.selected.transform.GetSiblingIndex() + 1, score);
    }

    void ShowLevelCard(int levelNum, int score)
    {
        GameText.text = "You have selected level: " + levelNum.ToString();
        StarPanel.ShowStars(score);
        PlayButton.gameObject.SetActive(true);
    }

    void ShowLockedLevel(int levelNum)
    {
        GameText.text = "This level is locked.\nBeat level " + (levelNum - 1).ToString() + " to unlock.";
    }

    private void OnDisable()
    {
        StarPanel.HideStars();
        PlayButton.gameObject.SetActive(false);
    }
}
