using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelAnoma : MonoBehaviour
{
    public int m_RequiredChains = 3;
    public GameObject m_UIPrefab;
    private ItemColour m_RequestedColour;
    private MelAnomaUI m_UIObject;

    // Use this for initialization
    void Awake()
    {
        GameManager.onScored += ColourScored;
        m_RequestedColour = (ItemColour)Random.Range(0, System.Enum.GetNames(typeof(ItemColour)).Length);

        m_UIObject = Instantiate(m_UIPrefab).GetComponent<MelAnomaUI>();
        m_UIObject.transform.SetParent(GameManager.instance.m_MainCanvas.transform, false);
        m_UIObject.SetText(m_RequestedColour);

        //Assign the delegates
        m_UIObject.m_ShowDone += ShowDone;
        m_UIObject.m_HideDone += HideDone;

        m_UIObject.Show();
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
            GameObject obj = GameManager.SpawnNodeItem();
            var bbros = obj.AddComponent<BacteriaBro>();
            bbros.m_Colour = obj.GetComponent<NodeItem>().m_Colour;
            DestroyImmediate(obj.GetComponent<NodeItem>());

            GameObject brosobj = Instantiate(GameManager.instance.m_BBros);

            //Position the bacteria bro
            Vector3 tempVec = brosobj.transform.position;
            brosobj.transform.SetParent(obj.transform);
            brosobj.transform.localPosition = tempVec;

            bbros.TakeBBInfo(brosobj.GetComponent<BacteriaBro>());
            DestroyImmediate(brosobj.GetComponent<BacteriaBro>());

            a_node.m_RespawnType = obj;
            a_node.m_RespawnIsSpawned = true;
        }
        --m_RequiredChains;

        m_RequestedColour = (ItemColour)Random.Range(0, System.Enum.GetNames(typeof(ItemColour)).Length);
        m_UIObject.SetText(m_RequestedColour);
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
            Destroy(m_UIObject);
            Destroy(this);
        }
    }
}
