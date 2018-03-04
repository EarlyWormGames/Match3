using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BG_Change : MonoBehaviour
{
    public Image img;
    public int[] Levels;
    public Sprite[] Sprites;

    private void Start()
    {
        if (Levels.Length != Sprites.Length)
        {
            Debug.LogError("Levels list differs from Sprite list count. Default sprite will be used");
            return;
        }

        for (int i = 0; i < Levels.Length; ++i)
        {
            if (Mediator.Settings.Level <= Levels[i] || i == Levels.Length - 1)
            {
                img.sprite = Sprites[i];
                break;
            }
        }
    }
}