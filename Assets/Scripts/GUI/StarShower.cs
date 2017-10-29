using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarShower : MonoBehaviour
{
    public Image[] Stars;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowStars(int amount)
    {
        amount = Mathf.Clamp(amount, 0, Stars.Length);
        //Default, just enable them
        for (int i = 0; i < amount; ++i)
        {
            Stars[i].gameObject.SetActive(true);
        }
    }

    public void HideStars()
    {
        for (int i = 0; i < Stars.Length; ++i)
        {
            Stars[i].gameObject.SetActive(false);
        }
    }
}
