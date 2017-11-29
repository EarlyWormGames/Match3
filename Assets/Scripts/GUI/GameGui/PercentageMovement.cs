using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageMovement : MonoBehaviour
{
    public bool Debug = false;
    [Range(0,1)]
    public float DebugScore = 0.5f;
    public AnimationCurve PercentagePosition;
    public float LerpSpeed = 1;

    public Image m_Image;
    public Image m_Tooth;
    public ParticleSystem m_PlusParticle;
    public ParticleSystem m_MinusParticle;
    public TrailRenderer m_Changer;
    public Transform BarStart;
    public Transform BarFinish;
    public Color GoodColor;
    public Color BadColor;

    internal float Percentage = 0.5f;

    GameObject ObjectToMove;
    bool NoErrors = true;

    float lastPercent;

    // Use this for initialization
    void Start()
    {
        NoErrors = true;
        if (ObjectToMove == null)
        {
            ObjectToMove = this.gameObject;
        }
        m_Image.fillAmount = Mathf.Lerp(0, 1, PercentagePosition.Evaluate(Debug ? DebugScore : Percentage));
        lastPercent = Percentage;
    }

    // Update is called once per frame
    void Update()
    {
        if (NoErrors)
        {
            //Clamping percentage 
            if (Percentage > 1)
            {
                Percentage = 1;
            }
            else if (Percentage < 0)
            {
                Percentage = 0;
            }

            if(Percentage == 1)
            {
                m_Image.color = Color.Lerp(m_Image.color,GoodColor, Time.deltaTime);
            }

            if(Percentage != lastPercent)
            {
                if(Percentage > lastPercent)
                {
                    m_PlusParticle.Play();
                }
                else
                {
                    m_MinusParticle.Play();
                }
                lastPercent = Percentage;
            }
            //Lerp the between the start and finish positions, using the Percentage
            float Lerp = Mathf.Lerp(0, 1, PercentagePosition.Evaluate(Debug ? DebugScore : Percentage));

            //Movement of the Object
            m_Image.fillAmount = Mathf.Lerp(m_Image.fillAmount, Lerp, Time.deltaTime * LerpSpeed);
            m_Tooth.fillAmount = m_Image.fillAmount;

            float TrailLerp = Mathf.Lerp(BarStart.position.x, BarFinish.position.x, PercentagePosition.Evaluate(Debug ? DebugScore : Percentage));
            m_Changer.transform.position = new Vector3(Mathf.Lerp(m_Changer.transform.position.x, TrailLerp, Time.deltaTime * LerpSpeed), m_Changer.transform.position.y, m_Changer.transform.position.z);

            
            m_Changer.startColor = Percentage < m_Image.fillAmount ? BadColor : GoodColor;
            m_Changer.endColor = Percentage < m_Image.fillAmount ? new Color(BadColor.r, BadColor.g, BadColor.b, 0.01f) : new Color(GoodColor.r, GoodColor.g, GoodColor.b, 0.01f);
        }
    }
}
