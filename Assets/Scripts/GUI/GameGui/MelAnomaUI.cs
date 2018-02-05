using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TextnColour
{
    public string text;
    public float FontSize = 72;
    public ItemColour itemType;
}

public class MelAnomaUI : BadGuyUI
{
    public TextnColour[] m_Colours;

    private float defaultFontSize = -1;

    public void SetText(ItemColour a_col)
    {
        if (defaultFontSize < 0)
            defaultFontSize = Text.fontSize;

        foreach (var item in m_Colours)
        {
            if (item.itemType == a_col)
            {
                Text.text = item.text;
                Text.fontSize = item.FontSize;
                break;
            }
        }
    }

    public void SetText(string a_text)
    {
        Text.text = a_text;
        Text.fontSize = defaultFontSize;
    }
}