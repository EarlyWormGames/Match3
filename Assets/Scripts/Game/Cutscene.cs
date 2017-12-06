using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Cutscene : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneEvent
    {
        public DirectorControlPlayable Timeline;
        public bool WaitForRefill = false;
    }
    public static Cutscene current;
    public CutsceneEvent[] events;
    private int i = -1;

    // Use this for initialization
    void Start()
    {
        current = this;
    }

    private void OnEnable()
    {
        GameManager.onRefill += OnRefilled;
    }

    private void OnDisable()
    {
        GameManager.onRefill -= OnRefilled;
    }

    void OnRefilled(bool a_wasSwap)
    {
        if (events[i].WaitForRefill)
            NextEvent();
    }

    public void NextEvent()
    {
        if (i >= 0)
            events[i].Timeline.director.Stop();
        i++;
        if (i < events.Length)
            events[i].Timeline.director.Play();
    }
}