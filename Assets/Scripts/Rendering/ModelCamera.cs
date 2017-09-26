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
        m_Texture = new RenderTexture(1024, 1024, 24);
        camera = GetComponent<Camera>();
        camera.targetTexture = m_Texture;
        camera.orthographic = true;
    }
}
