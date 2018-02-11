using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSaver : MonoBehaviour
{
    public ScrollRect ScrollToSave;

    private void Start()
    {
        ScrollToSave.normalizedPosition = SaveData.instance.ScrollPoint;
    }

    public void SaveSettings()
    {
        SaveData.instance.ScrollPoint = ScrollToSave.normalizedPosition;
        SaveData.Save();
    }
}