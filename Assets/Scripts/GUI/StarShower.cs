using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarShower : MonoBehaviour
{
    public Image[] Stars;
    public bool OnStart = false;
    public float StarDelay;

    private int rank;
    // Use this for initialization
    void Start()
    {
        if (OnStart)
        {
            int level = transform.parent.GetSiblingIndex();
            ShowStars(SaveData.LevelScores[level]);
        }
    }

    public void ShowStars(int amount)
    {
        rank = Mathf.Clamp(amount, 0, Stars.Length);
        //Default, just enable them
        StartCoroutine("StaggerStars");
    }

    public void HideStars()
    {
        for (int i = 0; i < Stars.Length; ++i)
        {
            Stars[i].gameObject.SetActive(false);
        }
    }

    IEnumerator StaggerStars()
    {
        for (int i = 0; i < Stars.Length; ++i)
        {
            Stars[i].gameObject.SetActive(i < rank);
            yield return new WaitForSeconds(StarDelay);
        }
    }
}
