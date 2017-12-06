using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Cutscene : MonoBehaviour
{
    [Serializable]
    public class CutsceneEvent
    {
        public PlayableDirector Timeline;
        public int WaitRefill = 0;
        public int WaitSeconds = 0;
        public bool WaitTouch = false;
        public bool WaitForEnd = true;

        [HideInInspector]
        public float RefillCount = 0;
    }
    public static Cutscene current;
    public static Action ClickEvent;

    public bool PlayOnAwake = false;
    public CutsceneEvent[] events;


    private int i = -1;
    private float timer = 0;

    // Use this for initialization
    void Start()
    {
        current = this;
        if (PlayOnAwake)
            NextEvent();
    }

    private void OnEnable()
    {
        GameManager.onRefill += OnRefilled;
    }

    private void OnDisable()
    {
        GameManager.onRefill -= OnRefilled;
    }

    private void Update()
    {
        if (i < events.Length)
        {
            if (events[i].WaitSeconds > 0)
            {
                if (events[i].WaitForEnd && events[i].Timeline.time < events[i].Timeline.duration)
                    return;

                timer += Time.deltaTime;

                //Start next after X seconds
                if (timer >= events[i].WaitSeconds)
                    NextEvent();
            }
        }
    }

    void OnRefilled(bool a_wasSwap)
    {
        if (i < events.Length)
        {
            if (events[i].WaitForEnd && events[i].Timeline.time < events[i].Timeline.duration)
                return;

            ++i;
            //Start next on refill amount
            if (events[i].RefillCount >= events[i].WaitRefill && events[i].WaitRefill > 0)
                NextEvent();
        }
    }

    public void OnClick()
    {
        //if (i < events.Length)
        //{
        //    if (events[i].WaitForEnd && events[i].Timeline.time < events[i].Timeline.duration)
        //        return;
        //
        //    if (events[i].WaitTouch)
        //        NextEvent();
        //}
        ClickEvent();
    }

    public void NextEvent()
    {
        if (i >= 0)
            events[i].Timeline.Stop();
        i++;
        if (i < events.Length)
            events[i].Timeline.Play();
    }
}