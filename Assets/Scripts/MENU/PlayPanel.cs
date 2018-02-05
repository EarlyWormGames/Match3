using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayPanel : MonoBehaviour
{
    public static PlayPanel instance;

    public StarShower StarPanel;
    public TextMeshProUGUI GameText;
    public Button PlayButton;
    public GameObject Renderer;
    public int dragThreshold = 10;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        Renderer.SetActive(false);
        StarPanel.HideStars();
    }

    private void Start()
    {
        Debug.Log(UnityEngine.EventSystems.EventSystem.current.pixelDragThreshold);
        UnityEngine.EventSystems.EventSystem.current.pixelDragThreshold = dragThreshold;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowLevelCard(int levelNum, int score)
    {
        GameText.text = "You have selected level: " + levelNum.ToString() + " Goal: " + LevelSettings.selected.TurnsGoal +  " Turns.";
        StarPanel.ShowStars(score);
        PlayButton.gameObject.SetActive(true);
    }

    void ShowLockedLevel(int levelNum)
    {
        GameText.text = "This level is locked.\nBeat level " + (levelNum - 1).ToString() + " to unlock.";
        PlayButton.gameObject.SetActive(false);
    }

    public void Show()
    {
        Renderer.SetActive(true);

        if (LevelSettings.selected == null)
            return;

        if (LevelSettings.selected.transform.GetSiblingIndex() > 0)
        {
            if (LevelSettings.selected.transform.GetSiblingIndex() - 1 >= SaveData.instance.LevelScores.Count)
            {
                ShowLockedLevel(LevelSettings.selected.transform.GetSiblingIndex() + 1);
                return;
            }
        }

        int score = 0;
        if (LevelSettings.selected.transform.GetSiblingIndex() < SaveData.instance.LevelScores.Count)
            score = SaveData.instance.LevelScores[LevelSettings.selected.transform.GetSiblingIndex()];

        ShowLevelCard(LevelSettings.selected.transform.GetSiblingIndex() + 1, score);
    }

    public void Hide()
    {
        Renderer.SetActive(false);
        StarPanel.HideStars();
        PlayButton.gameObject.SetActive(false);
    }
}
