using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TextnColour
{
    public string text;
    public Color colour = Color.white;
    public ItemColour itemType;
}

public class MelAnomaUI : BadGuyUI
{
    public TextnColour[] m_Colours;

    public void SetText(ItemColour a_col)
    {
        foreach (var item in m_Colours)
        {
            if (item.itemType == a_col)
            {
                Text.text = item.text;
                Text.color = item.colour;
                break;
            }
        }
    }
}