using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Direction
{
    None,
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
        if (!GameManager.isDragging || GameManager.dragObject == null)
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
        if (m_Left == a_other && a_other.m_Shape.CanSwap(Direction.Right) &&
            m_Shape.CanSwap(Direction.Left))
            m_Shape.Swap(a_other.m_Shape, Direction.Left);

        if (m_Right == a_other && a_other.m_Shape.CanSwap(Direction.Left) &&
            m_Shape.CanSwap(Direction.Right))
            m_Shape.Swap(a_other.m_Shape, Direction.Right);

        if (m_Up == a_other && a_other.m_Shape.CanSwap(Direction.Down) &&
            m_Shape.CanSwap(Direction.Up))
            m_Shape.Swap(a_other.m_Shape, Direction.Up);

        if (m_Down == a_other && a_other.m_Shape.CanSwap(Direction.Up) &&
            m_Shape.CanSwap(Direction.Down))
            m_Shape.Swap(a_other.m_Shape, Direction.Down);
    }

    public void TrySwap(Direction dir)
    {
        if (!m_Shape.CanSwap(dir))
            return;

        switch (dir)
        {
            case Direction.Left:
                if (m_Left != null)
                {
                    if (m_Left.m_Shape.CanSwap(Direction.Right))
                        m_Shape.Swap(m_Left.m_Shape, dir);
                }
                break;

            case Direction.Right:
                if (m_Right != null)
                {
                    if (m_Left.m_Shape.CanSwap(Direction.Left))
                        m_Shape.Swap(m_Right.m_Shape, dir);
                }
                break;

            case Direction.Up:
                if (m_Up != null)
                {
                    if (m_Left.m_Shape.CanSwap(Direction.Down))
                        m_Shape.Swap(m_Up.m_Shape, dir);
                }
                break;

            case Direction.Down:
                if (m_Down != null)
                {
                    if (m_Left.m_Shape.CanSwap(Direction.Up))
                        m_Shape.Swap(m_Down.m_Shape, dir);
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
        //if (m_Shape != null)
        //{
        //    if (m_Up != null)
        //        m_Up.TryTakeUp();
        //    return;
        //}
        GridNode node = null;
        if (m_Up != null)
            node = m_Up.FindNextAvailable();

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

    public void CheckMatch(Direction dir = Direction.None)
    {
        if (m_Shape == null)
            return;

        if (dir == Direction.None || dir == Direction.Left)
        {
            GameManager.NodeChainLeft.Add(this);
            if (m_Left != null)
            {
                if (m_Left.m_Shape != null)
                {
                    if (m_Left.m_Shape.m_Colour == m_Shape.m_Colour)
                    {
                        m_Left.CheckMatch(Direction.Left);
                    }
                }
            }
        }

        if (dir == Direction.None || dir == Direction.Right)
        {
            GameManager.NodeChainRight.Add(this);
            if (m_Right != null)
            {
                if (m_Right.m_Shape != null)
                {
                    if (m_Right.m_Shape.m_Colour == m_Shape.m_Colour)
                    {
                        m_Right.CheckMatch(Direction.Right);
                    }
                }
            }
        }

        if (dir == Direction.None || dir == Direction.Up)
        {
            GameManager.NodeChainUp.Add(this);
            if (m_Up != null)
            {
                if (m_Up.m_Shape != null)
                {
                    if (m_Up.m_Shape.m_Colour == m_Shape.m_Colour)
                    {
                        m_Up.CheckMatch(Direction.Up);
                    }
                }
            }
        }


        if (dir == Direction.None || dir == Direction.Down)
        {
            GameManager.NodeChainDown.Add(this);
            if (m_Down != null)
            {
                if (m_Down.m_Shape != null)
                {
                    if (m_Down.m_Shape.m_Colour == m_Shape.m_Colour)
                    {
                        m_Down.CheckMatch(Direction.Down);
                    }
                }
            }
        }

        if (dir == Direction.None)
        {
            //GameManager.NodeChainLeft.Add(this);
            //GameManager.NodeChainRight.Add(this);
            //GameManager.NodeChainUp.Add(this);
            //GameManager.NodeChainDown.Add(this);
            DestroyCheck();
        }
    }

    void DestroyCheck()
    {
        List<GridNode> destroynodes = new List<GridNode>();
        List<GridNode> leftright = new List<GridNode>();
        List<GridNode> updown = new List<GridNode>();

        foreach (var node in GameManager.NodeChainLeft)
        {
            if (!leftright.Contains(node))
                leftright.Add(node);
        }

        foreach (var node in GameManager.NodeChainRight)
        {
            if (!leftright.Contains(node))
                leftright.Add(node);
        }

        if (leftright.Count > 2)
            destroynodes.AddRange(leftright);





        foreach (var node in GameManager.NodeChainUp)
        {
            if (!updown.Contains(node))
                updown.Add(node);
        }

        foreach (var node in GameManager.NodeChainDown)
        {
            if (!updown.Contains(node))
                updown.Add(node);
        }

        if (updown.Count > 2)
            destroynodes.AddRange(updown);






        foreach (var node in destroynodes)
        {
            if (node.m_Shape != null)
            {
                ++GameManager.RespawnCounts[node.m_xIndex];
                DestroyImmediate(node.m_Shape.gameObject);
                ++GameManager.Score;
            }
        }

        GameManager.ClearNodeChains();
    }
}
