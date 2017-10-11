using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelAnoma : MonoBehaviour
{
    public static MelAnoma instance;

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
            GameObject obj = GameManager.SpawnNodeItem();
            obj.transform.position = new Vector3(100, 100, 100);
            var bbros = obj.AddComponent<BacteriaBro>();
            bbros.m_Colour = obj.GetComponent<NodeItem>().m_Colour;
            DestroyImmediate(obj.GetComponent<NodeItem>());

            GameObject brosobj = Instantiate(GameManager.instance.m_BBros);

            //Position the bacteria bro
            Vector3 tempVec = brosobj.transform.position;
            brosobj.transform.SetParent(obj.transform);
            brosobj.transform.localPosition = tempVec;

            //Take the info from the prefab, then destroy the script on it
            bbros.TakeBBInfo(brosobj.GetComponent<BacteriaBro>());
            DestroyImmediate(brosobj.GetComponent<BacteriaBro>());

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
            instance = null;
        }
    }

    void NewColour()
    {
        //Generate a new colour (definitely not the same)
        ItemColour prevcol = m_RequestedColour;
        int index = 0;
        do
        {
            index = Random.Range(0, m_UIObject.m_Colours.Length - 1);
            m_RequestedColour = m_UIObject.m_Colours[index].itemType;

        } while (m_RequestedColour == prevcol);

        m_UIObject.SetText(m_RequestedColour);
    }
}
