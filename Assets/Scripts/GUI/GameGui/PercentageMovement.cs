using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageMovement : MonoBehaviour
{
    public delegate void ValueChanged(float prev, float curr);


    public bool Debug = false;
    [Range(0,1)]
    public float DebugScore = 0.5f;
    public AnimationCurve PercentagePosition;
    public float LerpSpeed = 1;

    public Image m_Image;    
    public TrailRenderer m_Changer;
    public Transform BarStart;
    public Transform BarFinish;
    public Color GoodColor;
    public Color BadColor;

    public ValueChanged OnValueChanged;

    float Percentage = 0.5f;

    // Use this for initialization
    void Start()
    {
        OnValueChanged(Debug ? DebugScore : Percentage, Debug ? DebugScore : Percentage);
        m_Image.fillAmount = Mathf.Lerp(0, 1, PercentagePosition.Evaluate(Debug ? DebugScore : Percentage));
    }

    // Update is called once per frame
    void Update()
    {
        if(Percentage == 1)
        {
            m_Image.color = Color.Lerp(m_Image.color,GoodColor, Time.deltaTime);
        }

        //Lerp the between the start and finish positions, using the Percentage
        float Lerp = Mathf.Lerp(0, 1, PercentagePosition.Evaluate(Debug ? DebugScore : Percentage));

        //Movement of the Object
        m_Image.fillAmount = Mathf.Lerp(m_Image.fillAmount, Lerp, Time.deltaTime * LerpSpeed);

        float TrailLerp = Mathf.Lerp(BarStart.position.x, BarFinish.position.x, PercentagePosition.Evaluate(Debug ? DebugScore : Percentage));
        m_Changer.transform.position = new Vector3(Mathf.Lerp(m_Changer.transform.position.x, TrailLerp, Time.deltaTime * LerpSpeed), m_Changer.transform.position.y, m_Changer.transform.position.z);

            
        m_Changer.startColor = Percentage < m_Image.fillAmount ? BadColor : GoodColor;
        m_Changer.endColor = Percentage < m_Image.fillAmount ? new Color(BadColor.r, BadColor.g, BadColor.b, 0.01f) : new Color(GoodColor.r, GoodColor.g, GoodColor.b, 0.01f);
    }

    public void SetPercentage(float value)
    {
        value = Mathf.Clamp01(value);
        if (value == Percentage)
            return;

        OnValueChanged(Percentage, value);
        Percentage = value;
    }

    public float GetPercentage() { return Percentage; }
}
