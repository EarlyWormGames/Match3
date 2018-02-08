using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeController : MonoBehaviour
{
    public float FadeSpeed = 1;
    public float playingVolume = 1;
    public bool StartOn = true;

    private AudioSource src;
    private float timer;
    private float start, end;
    private bool started = false;

    // Use this for initialization
    void Start()
    {
        src = GetComponent<AudioSource>();

        if (StartOn)
            FadeIn();
        else
            FadeOut();
        timer = FadeSpeed;
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer = Mathf.Clamp(timer + Time.deltaTime, 0, FadeSpeed);
        src.volume = Mathf.Lerp(start, end, timer / FadeSpeed);
    }

    public void FadeOut()
    {
        if (start == playingVolume && started)
            return;

        start = playingVolume;
        end = 0;
        timer = FadeSpeed - timer;
    }

    public void FadeIn()
    {
        if (start == 0 && started)
            return;

        start = 0;
        end = playingVolume;
        timer = FadeSpeed - timer;
    }
}