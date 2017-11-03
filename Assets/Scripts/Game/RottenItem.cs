using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RottenItem : NodeItem
{
    [System.Serializable]
    public class SpriteType
    {
        public ItemColour colour;
        public Sprite sprite;
    }

    public Image m_MainImage;
    public SpriteType[] m_Sprites;

    protected override void OnStart()
    {
        base.OnStart();

        foreach (var item in m_Sprites)
        {
            if (item.colour == m_Colour)
            {
                m_MainImage.sprite = item.sprite;
            }
        }
    }

    private void OnEnable()
    {
        GameManager.onRefill += Refilled;
    }

    private void OnDisable()
    {
        GameManager.onRefill -= Refilled;
    }

    public void Refilled(bool a_wasSwap)
    {
        if (!a_wasSwap)
            return;

        if (m_Parent == null)
        {
            Destroy(gameObject);
            return;
        }

        List<GridNode> nodes = new List<GridNode>();
        if (m_Parent.HasDirection(Direction.Right, true))
        {
            if (m_Parent.m_Right.m_Shape.GetType() == typeof(NodeItem))
                nodes.Add(m_Parent.m_Right);
        }
        if (m_Parent.HasDirection(Direction.Left, true))
        {
            if (m_Parent.m_Left.m_Shape.GetType() == typeof(NodeItem))
                nodes.Add(m_Parent.m_Left);
        }
        if (m_Parent.HasDirection(Direction.Up, true))
        {
            if (m_Parent.m_Up.m_Shape.GetType() == typeof(NodeItem))
                nodes.Add(m_Parent.m_Up);
        }
        if (m_Parent.HasDirection(Direction.Down, true))
        {
            if (m_Parent.m_Down.m_Shape.GetType() == typeof(NodeItem))
                nodes.Add(m_Parent.m_Down);
        }

        if (nodes.Count > 0)
        {
            RottenItem spawn = Instantiate(GameManager.instance.m_RottenFood).GetComponent<RottenItem>();
            int rand = Random.Range(0, nodes.Count);
            spawn.m_Colour = nodes[rand].m_Shape.m_Colour;
            nodes[rand].m_RespawnType = spawn.gameObject;
            nodes[rand].m_RespawnIsSpawned = true;
            nodes[rand].StartDestroy(false);
        }
    }

    public override void OnEndDestroy()
    {
        if (m_bUseScore)
        {
            --GameManager.Score;
            GameManager.Score -= GameManager.LastChainGoodCount * 2;
        }
    }
}
