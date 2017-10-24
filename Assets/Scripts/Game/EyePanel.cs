using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyePanel : MonoBehaviour
{
    public static EyePanel instance;

    private void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}
