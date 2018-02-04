using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public enum EventType
    {
        P1_Text,
        P2_Text,
        SwitchP1,
        SwitchP2,
        TriggerEvent,
        SceneLoad,
    }

    [System.Serializable]
    public class DialogueEvent
    {
        public EventType type;
        public bool requireClick = true;
        public bool waitForMouse = true;
        public bool waitForTime = false;
        public float waitTime;
        public AudioClip audioClip;
        [Range(0, 1)] public float audioVolume = 1;
        [Multiline] public string ExtraData;
    }

    [System.Serializable]
    public class Portait
    {
        public Texture2D texture;
        public string name;
    }

    public List<Portait> Portraits = new List<Portait>();

    public DialogueEvent[] Events;

    public Texture2D GetTexture(string name)
    {
        foreach (var item in Portraits)
        {
            if (item.name == name)
            {
                return item.texture;
            }
        }
        return null;
    }
}