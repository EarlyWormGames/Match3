using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageMovement : MonoBehaviour
{

    public float Percentage = 0.5f;
    public float LerpSpeed = 1;

    public Image m_Image;

    GameObject ObjectToMove;
    bool NoErrors = true;

    // Use this for initialization
    void Start()
    {
        NoErrors = true;
        if (ObjectToMove == null)
        {
            ObjectToMove = this.gameObject;
        }
        m_Image.fillAmount = Mathf.Lerp(0, 1, Percentage);
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

            //Lerp the between the start and finish positions, using the Percentage
            float Lerp = Mathf.Lerp(0, 1, Percentage);

            //Movement of the Object
            m_Image.fillAmount = Mathf.Lerp(m_Image.fillAmount, Lerp, Time.deltaTime * LerpSpeed);
        }
    }
}
