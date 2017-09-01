﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public GameObject[] m_ShapePrefabs;
    public Vector3 m_ShapeScale = new Vector3(2, 2, 2);

    internal GameObject m_Shape;

    // Use this for initialization
    void Start()
    {
        int index = Random.Range(0, m_ShapePrefabs.Length);
        m_Shape = Instantiate(m_ShapePrefabs[index]);
        m_Shape.transform.parent = transform;
        m_Shape.transform.localScale = m_ShapeScale;
        m_Shape.transform.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
