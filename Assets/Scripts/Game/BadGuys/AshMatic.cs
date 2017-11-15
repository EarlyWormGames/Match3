using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshMatic : BadGuy
{
    public GameObject m_UIPrefab;
    public float m_WaitSeconds = 1;
    private AshMaticUI m_UIObject;
    private bool hideOnShow;

    public override void NoEffect()
    {
        hideOnShow = true;
    }

    // Use this for initialization
    void Start()
    {
        m_UIObject = Instantiate(m_UIPrefab).GetComponent<AshMaticUI>();
        m_UIObject.transform.SetParent(GameManager.instance.m_MainCanvas.transform, false);

        if (hideOnShow)
            Destroy(m_UIObject.transform.GetChild(0).gameObject);

        //Assign some delegates for the UI animating
        m_UIObject.m_ShowDone += ShowDone;
        m_UIObject.m_HideDone += HideDone;

        m_UIObject.Show();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowDone()
    {
        DoIt();
        StartCoroutine(WaitTimer());
    }

    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(m_WaitSeconds);
        m_UIObject.Hide();
    }

    void HideDone()
    {
        Destroy(m_UIObject.gameObject);
        Destroy(gameObject);
    }

    public void DoIt()
    {
        if (hideOnShow)
        {
            m_UIObject.Hide();
            return;
        }

        NodeItem item = null;
        do
        {
            int randX = Random.Range(0, Mediator.Settings.GridWidth);
            int randY = Random.Range(1, Mediator.Settings.GridHeight);

            item = GameManager.instance.m_Grid.m_Nodes[randX, randY].m_Shape;

            if (item != null)
                continue;

        } while (item.GetType() == typeof(NodeItem));

        RottenItem spawn = Instantiate(GameManager.instance.m_RottenFood).GetComponent<RottenItem>();
        spawn.m_Colour = item.m_Colour;

        item.m_Parent.m_RespawnType = spawn.gameObject;
        item.m_Parent.m_RespawnIsSpawned = true;
        item.m_Parent.StartDestroy(false);
    }
}
