using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshMatic : MonoBehaviour
{    
    public GameObject m_RottenFood;
    public GameObject m_UIPrefab;
    private AshMaticUI m_UIObject;

    // Use this for initialization
    void Start()
    {
        m_UIObject = Instantiate(m_UIPrefab).GetComponent<AshMaticUI>();
        m_UIObject.transform.SetParent(GameManager.instance.m_MainCanvas.transform, false);

        //Assign some delegates for the UI animating
        m_UIObject.m_ShowDone += ShowDone;
        m_UIObject.m_HideDone += HideDone;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowDone()
    {
        DoIt();
    }

    void HideDone()
    {
        Destroy(m_UIObject.gameObject);
        Destroy(gameObject);
    }

    public void DoIt()
    {
        NodeItem item = null;
        do
        {
            int randX = Random.Range(0, Mediator.Settings.GridWidth);
            int randY = Random.Range(0, Mediator.Settings.GridHeight);

            item = GameManager.instance.m_Grid.m_Nodes[randX, randY].m_Shape;

        } while (item.GetType() == typeof(NodeItem));

        RottenItem spawn = Instantiate(m_RottenFood).GetComponent<RottenItem>();
        spawn.m_Colour = item.m_Colour;

        item.m_Parent.m_RespawnType = spawn.gameObject;
        item.m_Parent.m_RespawnIsSpawned = true;
        item.m_Parent.StartDestroy(false);
    }
}
