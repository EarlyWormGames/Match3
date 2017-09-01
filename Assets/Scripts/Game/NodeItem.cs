using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemColour
{
    Blue,
    Green,
    Pink,
    Purple,
    Orange,
    Yellow,
    Red,
}

public class NodeItem : MonoBehaviour
{
    public ItemColour m_Colour;
    internal GridNode m_Parent;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_Parent.transform.position;
    }

    /// <summary>
    /// Swap this node with another
    /// </summary>
    /// <param name="a_other">The other node to swap with</param>
    public void Swap(NodeItem a_other)
    {
        //Swap the shape inside the parent
        a_other.m_Parent.m_Shape = this;
        m_Parent.m_Shape = a_other;

        //Swap the parents
        GridNode temp = a_other.m_Parent;
        a_other.m_Parent = m_Parent;
        m_Parent = temp;

        CheckMatch();
        a_other.CheckMatch();
    }

    public void CheckMatch(Direction? dir = null)
    {
        if (dir == null)
        {
            GameManager.NodeChainLeft.Add(m_Parent);
            GameManager.NodeChainRight.Add(m_Parent);
            GameManager.NodeChainUp.Add(m_Parent);
            GameManager.NodeChainDown.Add(m_Parent);

            //Check left
            if (m_Parent.m_Left != null)
            {
                if (m_Parent.m_Left.m_Shape.m_Colour == m_Colour)
                {
                    m_Parent.m_Left.m_Shape.CheckMatch(Direction.Left);
                }
            }

            //Check right
            if (m_Parent.m_Right != null)
            {
                if (m_Parent.m_Right.m_Shape.m_Colour == m_Colour)
                {
                    m_Parent.m_Right.m_Shape.CheckMatch(Direction.Right);
                }
            }

            //Check up
            if (m_Parent.m_Up != null)
            {
                if (m_Parent.m_Up.m_Shape.m_Colour == m_Colour)
                {
                    m_Parent.m_Up.m_Shape.CheckMatch(Direction.Up);
                }
            }

            //Check down
            if (m_Parent.m_Down != null)
            {
                if (m_Parent.m_Down.m_Shape.m_Colour == m_Colour)
                {
                    m_Parent.m_Down.m_Shape.CheckMatch(Direction.Down);
                }
            }
        }
        else
        {
            //Check the correct direction. Can only do cross shaped searches
            switch (dir.Value)
            {
                case Direction.Left:
                    GameManager.NodeChainLeft.Add(m_Parent);
                    if (m_Parent.m_Left != null)
                    {
                        if (m_Parent.m_Left.m_Shape.m_Colour == m_Colour)
                        {
                            m_Parent.m_Left.m_Shape.CheckMatch(Direction.Left);
                        }
                    }
                    break;

                case Direction.Right:
                    GameManager.NodeChainRight.Add(m_Parent);
                    if (m_Parent.m_Right != null)
                    {
                        if (m_Parent.m_Right.m_Shape.m_Colour == m_Colour)
                        {
                            m_Parent.m_Right.m_Shape.CheckMatch(Direction.Right);
                        }
                    }
                    break;

                case Direction.Up:
                    GameManager.NodeChainUp.Add(m_Parent);
                    if (m_Parent.m_Up != null)
                    {
                        if (m_Parent.m_Up.m_Shape.m_Colour == m_Colour)
                        {
                            m_Parent.m_Up.m_Shape.CheckMatch(Direction.Up);
                        }
                    }
                    break;

                case Direction.Down:
                    GameManager.NodeChainDown.Add(m_Parent);
                    if (m_Parent.m_Down != null)
                    {
                        if (m_Parent.m_Down.m_Shape.m_Colour == m_Colour)
                        {
                            m_Parent.m_Down.m_Shape.CheckMatch(Direction.Down);
                        }
                    }
                    break;
            }

            return;
        }

        //Only the initiator will get to here, so we'll do the scoring here

        //Left
        if (GameManager.NodeChainLeft.Count > 2)
        {
            GameManager.Score += GameManager.NodeChainLeft.Count;

            foreach (var node in GameManager.NodeChainLeft)
            {
                if (node.m_Shape == null)
                    continue;
                Destroy(node.m_Shape.gameObject);
                node.m_Shape = null;
            }
        }

        //Right
        if (GameManager.NodeChainRight.Count > 2)
        {
            GameManager.Score += GameManager.NodeChainRight.Count;

            foreach (var node in GameManager.NodeChainRight)
            {
                if (node.m_Shape == null)
                    continue;
                Destroy(node.m_Shape.gameObject);
                node.m_Shape = null;
            }
        }

        //Up
        if (GameManager.NodeChainUp.Count > 2)
        {
            GameManager.Score += GameManager.NodeChainUp.Count;

            foreach (var node in GameManager.NodeChainUp)
            {
                if (node.m_Shape == null)
                    continue;
                Destroy(node.m_Shape.gameObject);
                node.m_Shape = null;
            }
        }

        //Down
        if (GameManager.NodeChainDown.Count > 2)
        {
            GameManager.Score += GameManager.NodeChainDown.Count;

            foreach (var node in GameManager.NodeChainDown)
            {
                if (node.m_Shape == null)
                    continue;
                Destroy(node.m_Shape.gameObject);
                node.m_Shape = null;
            }
        }

        GameManager.instance.m_Grid.CheckColumns();

        //We're done with these, so clear them
        GameManager.NodeChainLeft.Clear();
        GameManager.NodeChainRight.Clear();
        GameManager.NodeChainUp.Clear();
        GameManager.NodeChainDown.Clear();
    }
}
