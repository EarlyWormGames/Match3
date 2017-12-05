using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionRender : MonoBehaviour
{

    // Use this for initialization
    void Update()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = "0.3.7";
    }
}
