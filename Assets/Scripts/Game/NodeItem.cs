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
    internal bool MarkDestroy;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((m_Parent.transform.position - transform.position).magnitude > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, m_Parent.transform.position, Time.deltaTime * GameManager.instance.m_NodeMoveSpeed);
            GameManager.Moving[m_Parent.m_xIndex, m_Parent.m_yIndex] = false;
        }
        else
        {
            GameManager.Moving[m_Parent.m_xIndex, m_Parent.m_yIndex] = true;
        }
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

        //m_Parent.CheckMatch();
        //a_other.CheckMatch();
    }
}
