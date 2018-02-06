using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimations : MonoBehaviour
{
    public DialogueDisplay Parent;

    public void NextDialogueEvent()
    {
        Parent.NextTrigger();
    }
}