using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class FadeImage : MonoBehaviour
{
    [Tooltip("Fade time in seconds")]
    public float FadeTime = 0.5f;
    public float TargetAlpha = 1;
    public UnityEvent OnFadeIn, OnFadeOut;
    private Graphic graphic;
    private bool fading, bIsFadeIn;
    private float timer;

    private void Start()
    {
        graphic = GetComponent<Graphic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            timer += Time.deltaTime;
            if (timer >= FadeTime)
            {
                timer = FadeTime;
                if (bIsFadeIn)
                    OnFadeIn.Invoke();
                else
                    OnFadeOut.Invoke();
                fading = false;
            }

            Color start = graphic.color;
            Color finish = graphic.color;
            start.a = bIsFadeIn ? 0 : TargetAlpha;
            finish.a = !bIsFadeIn ? 0 : TargetAlpha;

            graphic.color = Color.Lerp(start, finish, timer / FadeTime);
        }
    }

    public void FadeIn()
    {
        bIsFadeIn = true;
        fading = true;
        timer = 0;
    }

    public void FadeOut()
    {
        bIsFadeIn = false;
        fading = true;
        timer = 0;
    }
}