using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }

    [System.Serializable]
    public class TriggerEvent
    {
        public string name;
        public UnityEvent trigger;
    }

    [System.Serializable]
    public class TextEvent
    {
        public string name;
        public StringEvent trigger;
    }

    [System.Serializable]
    public class DialogueEvent
    {
        public string TriggerName;
        [Multiline] public string Data;
        public bool requireClick = true;
        [Tooltip("Wait for another script or animation to trigger the next event")]
        public bool waitForTrigger = false;
        public float waitTime;
    }

    public bool IgnoreCase = true;
    public DialogueEvent[] Events;
}