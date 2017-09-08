using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelAnoma : MonoBehaviour
{
    public int m_RequiredChains = 3;
    private ItemColour m_RequestedColour;

    // Use this for initialization
    void Awake()
    {
        GameManager.onScored += ColourScored;
        m_RequestedColour = (ItemColour)Random.Range(0, System.Enum.GetNames(typeof(ItemColour)).Length);
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
        if (!a_wasSwapped || m_RequiredChains == 0)
            return;

        if (a_colour == m_RequestedColour)
        {
            //GOOD!
            m_RequiredChains = 0;
        }
        else
        {
            //BAD!!!
            a_node.m_RespawnType = GameManager.instance.m_BBros;
        }
        --m_RequiredChains;

        m_RequestedColour = (ItemColour)Random.Range(0, System.Enum.GetNames(typeof(ItemColour)).Length);
    }

    void End()
    {

    }
}
