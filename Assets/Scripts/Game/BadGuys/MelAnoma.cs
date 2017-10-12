using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelAnoma : MonoBehaviour
{
    public int m_RequiredChains = 3;
    public GameObject m_UIPrefab;
    private ItemColour m_RequestedColour = ItemColour.NONE;
    private MelAnomaUI m_UIObject;

    // Use this for initialization
    void Awake()
    {
        //Get a delegate callback when a colour has been scored
        GameManager.onScored += ColourScored;

        //Create our new UI object
        m_UIObject = Instantiate(m_UIPrefab).GetComponent<MelAnomaUI>();
        m_UIObject.transform.SetParent(GameManager.instance.m_MainCanvas.transform, false);

        //Generate a new colour request
        NewColour();

        //Assign some delegates for the UI animating
        m_UIObject.m_ShowDone += ShowDone;
        m_UIObject.m_HideDone += HideDone;

        //Show our UI object
        m_UIObject.Show();
    }

    private void OnDestroy()
    {
        //Unattach the function from the delegate
        GameManager.onScored -= ColourScored;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ColourScored(ItemColour a_colour, bool a_wasSwapped, GridNode a_node)
    {
        if (!a_wasSwapped || m_RequiredChains <= 0)
            return;

        if (a_colour == m_RequestedColour)
        {
            //GOOD!
            End();
            return;
        }
        else
        {
            //BAD!!!
            //Create a new Bacteria Bro
            int index = GameManager.GetNodeIndex();
            NodeItem prefab = GameManager.GetNodeDetails(index);
            GameObject obj = Instantiate(GameManager.instance.m_BBros);
            BacteriaBro bbro = obj.GetComponent<BacteriaBro>();
            bbro.m_Colour = prefab.m_Colour;

            bbro.m_RespawnType = prefab.gameObject;

            GameObject child = GameManager.SpawnNodeItem(index);
            child.transform.SetParent(bbro.transform, false);
            child.transform.SetAsFirstSibling();

            bbro.transform.SetParent(GameManager.instance.m_Grid.transform, false);

            //Tell it that it has a respawn object so it doesn't make the grid move
            a_node.m_RespawnType = obj;
            a_node.m_RespawnIsSpawned = true;
        }
        --m_RequiredChains;

        if (m_RequiredChains <= 0)
        {
            End();
            return;
        }

        //Generate a new colour and tell the UI object
        NewColour();
    }

    void End()
    {
        m_RequiredChains = 0;
        m_UIObject.Hide();
    }

    void ShowDone()
    {

    }

    void HideDone()
    {
        if (m_RequiredChains <= 0)
        {
            Destroy(m_UIObject.gameObject);
            Destroy(gameObject);
        }
    }

    void NewColour()
    {
        //Generate a new colour (definitely not the same)
        ItemColour prevcol = m_RequestedColour;
        int index = 0;
        do
        {
            index = Random.Range(0, m_UIObject.m_Colours.Length);
            m_RequestedColour = m_UIObject.m_Colours[index].itemType;

        } while (m_RequestedColour == prevcol);

        m_UIObject.SetText(m_RequestedColour);
    }
}
