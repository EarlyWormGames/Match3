using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToothChanger : MonoBehaviour
{
    [System.Serializable]
    public class Change
    {
        public Sprite sprite;
        public float percentage;
    }

    public PercentageMovement healthBar;
    public Image m_Tooth;
    public ParticleSystem m_PlusParticle;
    public ParticleSystem m_MinusParticle;
    public Change[] m_Changes;

    private void OnEnable()
    {
        healthBar.OnValueChanged += ValueChanged;
    }

    private void OnDisable()
    {
        healthBar.OnValueChanged -= ValueChanged;
    }

    void ValueChanged(float prev, float curr)
    {
        if (curr > prev)
        {
            m_PlusParticle.Play();
        }
        else
        {
            m_MinusParticle.Play();
        }

        foreach (var item in m_Changes)
        {
            if (curr >= item.percentage)
            {
                m_Tooth.sprite = item.sprite;
            }
        }
    }
}
