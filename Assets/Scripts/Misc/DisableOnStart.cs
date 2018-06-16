using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.SetActive(false);
        Destroy(this);
    }
}
