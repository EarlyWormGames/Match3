using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using System.Linq;

public class DialogueDisplay : MonoBehaviour
{
    public Dialogue Asset;
    public Dialogue.TextEvent[] TextEvents;

    public UnityEvent OnFinished;

    private int CurrentIndex = -1;
    private bool playing;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (Asset.Events[CurrentIndex].requireClick)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    NextTrigger();
                }
            }
            else if (!Asset.Events[CurrentIndex].waitForTrigger)
            {
                timer += Time.deltaTime;
                if (timer >= Asset.Events[CurrentIndex].waitTime)
                {
                    NextTrigger();
                }
            }
        }
    }

    public void Play()
    {
        playing = true;
        timer = 0;
        CurrentIndex = -1;
        NextTrigger();
    }

    public void NextTrigger()
    {
        ++CurrentIndex;
        if (CurrentIndex < Asset.Events.Length)
        {
            //Pull the trigger name from the asset
            string triggerName = Asset.Events[CurrentIndex].TriggerName;
            if (Asset.IgnoreCase)
                triggerName = triggerName.ToLower();

            foreach (var item in TextEvents)
            {
                string itemName = item.name;
                if (Asset.IgnoreCase)
                    itemName = itemName.ToLower();

                //Find the corresponding trigger
                if (itemName == triggerName)
                {
                    //Call the function and pass in the extra data
                    //e.g. An animation variable to set, text so set on a UI Text object etc
                    item.trigger.Invoke(Asset.Events[CurrentIndex].Data);
                }
            }
        }
        else
        {
            playing = false;
            OnFinished.Invoke();
        }
    }
}