using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisabler : MonoBehaviour
{

    // Use this for initialization
    void OnEnable()
    {
        if (!SaveData.IsDev)
            gameObject.SetActive(false);
    }
}
