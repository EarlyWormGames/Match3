using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelAnoma : BadGuy
{
    public int m_RequiredChains = 3;
    public GameObject m_UIPrefab;
    private ItemColour m_RequestedColour = ItemColour.NONE;
    private MelAnomaUI m_UIObject;

    private List<BacteriaBro> bros = new List<BacteriaBro>();
    private bool hideOnShow, finished;

    public override void NoEffect()
    {
        hideOnShow = true;
    }

    // Use this for initialization
    void Awake()
    {
        //Get a delegate callback when a colour has been scored
        GameManager.onScored += ColourScored;

        //Create our new UI object
        m_UIObject = Instantiate(m_UIPrefab).GetComponent<MelAnomaUI>();
        m_UIObject.transform.SetParent(GameManager.instance.m_MainCanvas.transform, false);

        if (hideOnShow)
            Destroy(m_UIObject.transform.GetChild(0).gameObject);

        //Assign some delegates for the UI animating
        m_UIObject.m_ShowDone += ShowDone;
        m_UIObject.m_HideDone += HideDone;
        m_UIObject.m_ShrinkDone += ShrinkDone;

        //Show our UI object
        m_UIObject.Show();

        CharacterShower.FadeIn();
    }

    private void OnDestroy()
    {
        //Unattach the function from the delegate
        GameManager.onScored -= ColourScored;
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (!finished)
                {
                    if (hideOnShow)
                    {
                        End();
                        CharacterShower.FadeOut();
                    }
                    else
                    {
                        m_UIObject.Shrink();
                        //Generate a new colour request
                        NewColour();
                    }
                }
                else
                {
                    FullEnd();
                }

                waiting = false;
            }
        }
    }

    void ColourScored(ItemColour a_colour, bool a_wasSwapped, GridNode a_node)
    {
        if (!a_wasSwapped || m_RequiredChains <= 0 || hideOnShow)
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
            GameObject obj = Instantiate(GameManager.instance.m_BBros, a_node.transform.position, new Quaternion(), GameManager.instance.m_Grid.transform);
            BacteriaBro bbro = obj.GetComponent<BacteriaBro>();
            bbro.m_Colour = prefab.m_Colour;

            bbro.m_RespawnType = prefab.gameObject;
            bbro.gameObject.SetActive(false);

            GameObject child = GameManager.SpawnNodeItem(index);
            child.transform.SetParent(bbro.transform, false);
            child.transform.SetAsFirstSibling();

            //Tell it that it has a respawn object so it doesn't make the grid move
            a_node.m_RespawnType = obj;
            a_node.m_RespawnIsSpawned = true;

            bros.Add(bbro);
        }
        --m_RequiredChains;

        if (m_RequiredChains <= 0)
        {
            End();
            return;
        }

        //Generate a new colour and tell the UI object
        //NewColour();
    }

    void End()
    {
        CharacterShower.FadeIn();

        string finalMessage = "Drats!";
        if (m_RequiredChains <= 0)
            finalMessage = "Go forth, Bacteria Bros!";

        m_UIObject.SetText(finalMessage);
        m_UIObject.UnShrink();

        finished = true;
        waiting = true;

        m_RequiredChains = 0;
    }

    void FullEnd()
    {
        m_UIObject.Hide();

        foreach (var bro in bros)
        {
            if (bro != null)
                bro.AllowCountUp = true;
        }
    }

    void ShowDone()
    {
        waiting = true;
    }

    void ShrinkDone()
    {
        if (!finished)
            CharacterShower.FadeOut();
    }

    void HideDone()
    {
        Destroy(m_UIObject.gameObject);
        Destroy(gameObject);
        CharacterShower.FadeOut();
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
