using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : MonoBehaviour
{
    public static PlayPanel instance;

    public StarShower StarPanel;
    public Text GameText;

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
        StarPanel.ShowStars(SaveData.LevelScores[LevelSettings.selected.transform.GetSiblingIndex()]);
        GameText.text = "You have selected level: " + LevelSettings.selected.transform.GetSiblingIndex().ToString();
    }

    private void OnDisable()
    {
        StarPanel.HideStars();
    }
}
