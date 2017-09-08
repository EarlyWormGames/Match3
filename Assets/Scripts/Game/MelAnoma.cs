using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelAnoma : MonoBehaviour
{
    private ItemColour m_RequestedColour;

    // Use this for initialization
    void Awake()
    {
        GameManager.onScored += ColourScored;
    }

    private void OnDestroy()
    {
        GameManager.onScored -= ColourScored;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ColourScored(ItemColour a_colour, bool a_wasSwapped, GridNode a_node)
    {
        if (!a_wasSwapped)
            return;

        if (a_colour == m_RequestedColour)
        {
            //GOOD!
        }
        else
        {
            //BAD!!!
        }
    }
}
