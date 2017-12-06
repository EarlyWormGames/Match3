using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneActivator : MonoBehaviour
{
    public void Next()
    {
        if (Cutscene.current == null)
            return;

        Cutscene.current.NextEvent();
    }
}
