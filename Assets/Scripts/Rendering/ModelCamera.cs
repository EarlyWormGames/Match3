using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCamera : MonoBehaviour
{
    public RenderTexture m_Texture;
    public int RenderCount;
    new public Camera camera;

    // Use this for initialization
    void Awake()
    {
        m_Texture = new RenderTexture(128, 128, 24);
        camera = GetComponent<Camera>();
        camera.targetTexture = m_Texture;
        camera.orthographic = true;
        camera.orthographicSize = 1.6f;
        camera.enabled = false;
    }
}
