using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ModelRenderer : MonoBehaviour
{
    public int m_RenderIndex;
    private RawImage m_Renderer;

    // Use this for initialization
    void Start()
    {
        m_Renderer = GetComponent<RawImage>();
        m_Renderer.texture = ModelSpawner.instance.GetTexture();

        m_Renderer.uvRect = ModelSpawner.instance.UsePortion(m_RenderIndex);
    }
}
