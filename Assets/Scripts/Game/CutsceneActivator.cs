using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneActivator : MonoBehaviour
{
    private void OnEnable()
    {
        if (Cutscene.current == null)
            return;

        Cutscene.current.NextEvent();
        gameObject.SetActive(false);
    }
}
