using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSettings : MonoBehaviour
{
    [System.Serializable]
    public class Row
    {
        public GameObject[] Prefabs;
    }

    public static LevelSettings selected;

    public int RequiredChain = 4;

    public int ColourChance = 1;

    public int GridWidth = 5;
    public int GridHeight = 7;

    public int TargetScore = 100;
    public float DifficultyMult = 1;

    public int TurnsGoal = 15;
    public int LevelNum;

    public bool isArcade;
    public bool AllowsEnemies = true;

    public Row[] Rows = new Row[0];

    public GameObject[] BlacklistedPrefabs = new GameObject[0];

    private void Start()
    {
        LevelNum = transform.GetSiblingIndex();
    }

    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveListener(OnClick);
    }

    protected virtual void OnClick()
    {
        selected = this;
        PlayPanel.instance.Show();
    }

    public GameObject[] JoinRows()
    {
        List<GameObject> joined = new List<GameObject>();
        foreach (var row in Rows)
        {
            joined.AddRange(row.Prefabs);
        }
        return joined.ToArray();
    }
}
