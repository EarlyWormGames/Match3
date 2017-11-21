using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScroller : MonoBehaviour
{
    public RectTransform Top;
    public RectTransform Bottom;
    public float ScrollSpeed = 10;
    public float Dampening = 0.8f;
    public InfinitePanel[] Panels;

    private int CurrentPanel;
    private int Count;

    private float Velocity;
    private Vector3 lastPos;
    private bool wasDown;
    private bool TriedSwap;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Velocity != 0)
        {
            Velocity *= Dampening;
        }

        if (Input.GetMouseButton(0))
        {
            if (wasDown)
            {
                //calculate velocity
                Velocity += (Input.mousePosition - lastPos).y * ScrollSpeed;
            }
            wasDown = true;
            lastPos = Input.mousePosition;
        }
        else
        {
            wasDown = false;
        }

        if (Velocity < 0)
            TriedSwap = false;

        for (int i = 0; i < Panels.Length; i++)
        {
            if (Velocity < 0)
            {
                if (Panels[i].Top.position.y < Bottom.position.y)
                {
                    ++Count;
                    int index = i + Panels.Length - 1;
                    if (index >= Panels.Length)
                        index -= Panels.Length;

                    if (Panels[i].TopLink != null)
                        Panels[i].TopLink.BottomLink = null;

                    Panels[i].TopLink = null;
                    Panels[i].BottomLink = Panels[index];
                    Panels[index].TopLink = Panels[i];

                    Panels[i].transform.position += Panels[index].Top.position - Panels[i].Bottom.position;
                }
            }
            else if(Velocity > 0)
            {
                if (Panels[i].Bottom.position.y > Top.position.y)
                {
                    if (Count <= 0)
                    {
                        TriedSwap = true;
                        Panels[i].BottomLink.transform.position += Top.position - Panels[i].BottomLink.Top.position;
                    }
                    else
                    {
                        --Count;
                        int index = i + 1;
                        if (index >= Panels.Length)
                            index -= Panels.Length;

                        if (Panels[i].BottomLink != null)
                            Panels[i].BottomLink.TopLink = null;

                        Panels[i].BottomLink = null;
                        Panels[i].TopLink = Panels[index];
                        Panels[index].BottomLink = Panels[i];

                        Panels[i].transform.position += Panels[index].Bottom.position - Panels[i].Top.position;
                    }
                }
            }


            if (!TriedSwap)
                Panels[i].transform.position += new Vector3(0, Velocity * Time.deltaTime, 0);
        }
    }
}