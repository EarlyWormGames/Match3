using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextByIndex : MonoBehaviour
{
    public int UseParentLevel = 2;
    public int AddAmount = 1;

    // Use this for initialization
    void Start()
    {
        Transform t = transform;
        for (int i = 0; i < UseParentLevel; ++i)
        {
            if (t.parent != null)
                t = t.parent;
        }

        GetComponent<TextMeshProUGUI>().text = (t.GetSiblingIndex() + AddAmount).ToString();
    }
}
