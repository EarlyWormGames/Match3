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
    public GameObject Renderer;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        Renderer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

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

    public void Show()
    {
        Renderer.SetActive(true);

        if (LevelSettings.selected == null)
            return;

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

    public void Hide()
    {
        Renderer.SetActive(false);
        StarPanel.HideStars();
        PlayButton.gameObject.SetActive(false);
    }
}
