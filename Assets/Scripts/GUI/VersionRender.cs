using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionRender : MonoBehaviour
{

    // Use this for initialization
    void Update()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = string.IsNullOrEmpty(Application.version) ? "0.3.7" : Application.version;
    }
}
