using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSettings : MonoBehaviour
{
    public static LevelSettings selected;

    public int RequiredChain = 4;

    public int ColourChance = 1;

    public int GridWidth = 5;
    public int GridHeight = 7;

    public int TargetScore = 100;
    public float DifficultyMult = 1;

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    void OnClick()
    {
        selected = this;
        PlayPanel.instance.gameObject.SetActive(true);
    }
}
