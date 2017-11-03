using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionRender : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = Application.version;
    }
}
