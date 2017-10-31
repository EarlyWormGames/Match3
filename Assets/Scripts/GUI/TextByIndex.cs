using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextByIndex : MonoBehaviour
{
    public bool UseParent = true;
    public int AddAmount = 1;

    // Use this for initialization
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = ((UseParent? transform.parent.GetSiblingIndex() : transform.GetSiblingIndex()) + AddAmount).ToString();
    }
}
