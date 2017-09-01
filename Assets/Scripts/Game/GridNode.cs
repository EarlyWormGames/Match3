using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}

public class GridNode : MonoBehaviour
{
    public GameObject[] m_ShapePrefabs;
    public Vector3 m_ShapeScale = new Vector3(2, 2, 2);

    internal NodeItem m_Shape;
    internal GridNode m_Left, m_Right, m_Up, m_Down;
    internal int m_xIndex, m_yIndex;

    // Use this for initialization
    void Start()
    {
        int index = Random.Range(0, m_ShapePrefabs.Length);
        GameObject obj = Instantiate(m_ShapePrefabs[index]);
        m_Shape = obj.GetComponent<NodeItem>();
        m_Shape.transform.parent = transform.parent;
        m_Shape.transform.localScale = m_ShapeScale;
        m_Shape.transform.localPosition = transform.position;
        m_Shape.m_Parent = this;

        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    // Update is called once per frame
    //void Update()
    //{
    //
    //}

    public void MouseDown(BaseEventData eventData)
    {
        GameManager.isDragging = true;
        GameManager.dragObject = gameObject;
        GameManager.dragStartPos = Input.mousePosition;
    }

    public void MouseUp(BaseEventData eventData)
    {
        if (!GameManager.isDragging)
            return;

        //Calculate the direction of the drag
        Vector3 dir = Input.mousePosition - GameManager.dragStartPos;
        dir.Normalize();

        //Get the node the drag started on
        GridNode node = GameManager.dragObject.GetComponent<GridNode>();

        //Convert these values to absolute values
        float x = dir.x;
        if (x < 0)
            x *= -1;
        float y = dir.y;
        if (y < 0)
            y *= -1;

        if (x > y)
        {
            if (dir.x < 0)
            {
                //Do left
                node.TrySwap(Direction.Left);
            }
            else
            {
                //Do right
                node.TrySwap(Direction.Right);
            }
        }
        else
        {
            if (dir.y < 0)
            {
                //Do down
                node.TrySwap(Direction.Down);
            }
            else
            {
                //Do up
                node.TrySwap(Direction.Up);
            }
        }

        //Finished, reset this data
        GameManager.dragStartPos = Vector3.zero;
        GameManager.isDragging = false;
        GameManager.dragObject = null;
    }

    public void MouseClick(BaseEventData eventData)
    {
        if (GameManager.dragObject == null)
            GameManager.dragObject = gameObject;
        else
        {
            GameManager.dragObject.GetComponent<GridNode>().TrySwap(this);
            GameManager.dragObject = null;
        }
    }

    public void TrySwap(GridNode a_other)
    {
        if (m_Left == a_other || m_Right == a_other ||
            m_Up == a_other || m_Down == a_other)
            m_Shape.Swap(a_other.m_Shape);
    }

    public void TrySwap(Direction dir)
    {
        switch (dir)
        {
            case Direction.Left:
                if (m_Left != null)
                {
                    m_Shape.Swap(m_Left.m_Shape);
                }
                break;

            case Direction.Right:
                if (m_Right != null)
                {
                    m_Shape.Swap(m_Right.m_Shape);
                }
                break;

            case Direction.Up:
                if (m_Up != null)
                {
                    m_Shape.Swap(m_Up.m_Shape);
                }
                break;

            case Direction.Down:
                if (m_Down != null)
                {
                    m_Shape.Swap(m_Down.m_Shape);
                }
                break;
        }
    }

    public void SpawnShape(Vector3 a_position)
    {
        int index = Random.Range(0, m_ShapePrefabs.Length);
        GameObject obj = Instantiate(m_ShapePrefabs[index]);
        m_Shape = obj.GetComponent<NodeItem>();
        m_Shape.transform.parent = transform.parent;
        m_Shape.transform.localScale = m_ShapeScale;
        m_Shape.transform.position = a_position;
        m_Shape.m_Parent = this;
    }

    public void Take(ref GridNode a_other)
    {
        m_Shape = a_other.m_Shape;
        m_Shape.m_Parent = this;
        a_other.m_Shape = null;
    }

    public GridNode FindLowestEmpty()
    {
        if (m_Shape == null)
        {
            if (m_Down != null)
                return m_Down.FindLowestEmpty();
            else
                return this;
        }
        else
        {
            return m_Up;
        }
    }

    public void TryTakeUp()
    {
        if (m_Shape != null)
        {
            if (m_Up != null)
                m_Up.TryTakeUp();
            return;
        }

        var node = FindNextAvailable();

        if (node != null)
            Take(ref node);

        //if (m_Up != null)
        //{
        //    if (m_Up.m_Shape != null && m_Shape == null)
        //        Take(ref m_Up);
        //
        //    m_Up.TryTakeUp();
        //}
    }

    public GridNode FindNextAvailable()
    {
        if (m_Shape != null)
            return this;
        if (m_Up != null)
            return m_Up.FindNextAvailable();

        return null;
    }
}
