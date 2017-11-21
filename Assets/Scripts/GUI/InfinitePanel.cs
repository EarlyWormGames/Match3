using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePanel : MonoBehaviour
{
    public RectTransform Top;
    public RectTransform Bottom;
    public InfinitePanel BottomLink;
    public InfinitePanel TopLink;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BottomLink != null)
        {
            transform.position += BottomLink.Top.position - Bottom.position;
        }
    }
}