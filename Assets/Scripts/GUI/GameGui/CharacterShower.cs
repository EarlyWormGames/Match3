using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShower : MonoBehaviour
{
    public static CharacterShower instance;
    public static float FadeSpeed = 1;
    private static bool fading, fadeForward;
    private static float timer;

    public CanvasGroup Fader;

    // Use this for initialization
    void Start()
    {
        instance = this;
        Fader.blocksRaycasts = false;
        Fader.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            timer += Time.deltaTime;
            Fader.alpha = Mathf.Lerp(fadeForward ? 0 : 1, fadeForward ? 1 : 0, timer / FadeSpeed);
            if (timer >= FadeSpeed)
            {
                fading = false;
                if (!fadeForward)
                    instance.Fader.blocksRaycasts = false;
            }
        }
    }

    public static void FadeIn()
    {
        timer = 0;
        fading = true;
        fadeForward = true;
        instance.Fader.blocksRaycasts = true;
    }

    public static void FadeOut()
    {
        fading = true;
        fadeForward = false;
        timer = FadeSpeed - timer;
    }
}
