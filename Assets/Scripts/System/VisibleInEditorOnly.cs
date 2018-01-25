using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleInEditorOnly : MonoBehaviour {

    private void Awake()
    {
        if (Debug.isDebugBuild)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
