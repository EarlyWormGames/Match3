using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RottenItem : NodeItem
{
    [System.Serializable]
    public class SpriteType
    {
        public ItemColour colour;
        public Sprite sprite;
    }

    public Image m_MainImage;
    public SpriteType[] m_Sprites;

    protected override void OnStart()
    {
        base.OnStart();

        foreach (var item in m_Sprites)
        {
            if (item.colour == m_Colour)
            {
                m_MainImage.sprite = item.sprite;
            }
        }
    }

    public override void OnEndDestroy()
    {
        if (m_bUseScore)
            --GameManager.Score;
    }
}
