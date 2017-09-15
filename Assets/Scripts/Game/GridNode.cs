using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public Vector3 m_ShapeScale = new Vector3(2, 2, 2);
    public Color m_HighlightColour;

    internal NodeItem m_Shape;
    internal GridNode m_Left, m_Right, m_Up, m_Down;
    internal int m_xIndex, m_yIndex;
    internal Image m_Image;
    internal GameObject m_RespawnType = null;
    internal bool m_RespawnIsSpawned = false;

    public void Init()
    {
        //This do-while ensures that this tile won't spawn in a chain
        //It will then spawn the tile
        int skipCount = -1;
        do
        {
            ++skipCount;
            int index = GameManager.GetNodeIndex();
            NodeItem ex = GameManager.GetNodeDetails(index);

            if (skipCount < 10)
            {
                if (m_Left != null)
                {
                    if (m_Left.m_Shape != null)
                    {
                        if (m_Left.m_Shape.CheckColour(ex))
                            continue;
                    }
                }

                if (m_Right != null)
                {
                    if (m_Right.m_Shape != null)
                    {
                        if (m_Right.m_Shape.CheckColour(ex))
                            continue;
                    }
                }

                if (m_Up != null)
                {
                    if (m_Up.m_Shape != null)
                    {
                        if (m_Up.m_Shape.CheckColour(ex))
                            continue;
                    }
                }

                if (m_Down != null)
                {
                    if (m_Down.m_Shape != null)
                    {
                        if (m_Down.m_Shape.CheckColour(ex))
                            continue;
                    }
                }
            }

            //If it gets here, it will spawn an item
            GameObject obj = GameManager.SpawnNodeItem(index);
            m_Shape = obj.GetComponent<NodeItem>();
            m_Shape.transform.parent = transform.parent;
            m_Shape.transform.localScale = m_ShapeScale;
            m_Shape.transform.localPosition = transform.position;
            m_Shape.m_Parent = this;

            break;

        } while (true);
    }

    public void DestroyNode()
    {
        DestroyImmediate(m_Shape.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        m_Image = GetComponent<Image>();
        m_RespawnType = null;
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isDragging)
        {
            if (GameManager.dragGNode == this)
            {
                //We're being dragged
                Vector3 dir = Input.mousePosition - GameManager.dragStartPos;
                if (dir.magnitude > 20f)
                {
                    //Drag is far enough, calculate which node to swap with
                    dir.Normalize();

                    float x = dir.x;
                    if (x < 0)
                        x *= -1;
                    float y = dir.y;
                    if (y < 0)
                        y *= -1;

                    bool unswap = false;

                    if (x > y)
                    {
                        if (dir.x < 0)
                        {
                            if (GameManager.lastDrag == Direction.None)
                            {
                                //Do left
                                if (m_Left != null)
                                {
                                    if (m_Left.m_Shape.CanSwap())
                                    {
                                        m_Shape.Swap(m_Left.m_Shape, Direction.Left);
                                        GameManager.lastDrag = Direction.Left;
                                    }
                                }
                            }
                            else if (GameManager.lastDrag != Direction.Left)
                                unswap = true;
                        }
                        else
                        {
                            if (GameManager.lastDrag == Direction.None)
                            {
                                //Do left
                                if (m_Right != null)
                                {
                                    if (m_Right.m_Shape.CanSwap())
                                    {
                                        m_Shape.Swap(m_Right.m_Shape, Direction.Right);
                                        GameManager.lastDrag = Direction.Right;
                                    }
                                }
                            }
                            else if (GameManager.lastDrag != Direction.Right)
                                unswap = true;
                        }
                    }
                    else
                    {
                        if (dir.y < 0)
                        {
                            if (GameManager.lastDrag == Direction.None)
                            {
                                //Do left
                                if (m_Down != null)
                                {
                                    if (m_Down.m_Shape.CanSwap())
                                    {
                                        m_Shape.Swap(m_Down.m_Shape, Direction.Down);
                                        GameManager.lastDrag = Direction.Down;
                                    }
                                }
                            }
                            else if (GameManager.lastDrag != Direction.Down)
                                unswap = true;
                        }
                        else
                        {
                            if (GameManager.lastDrag == Direction.None)
                            {
                                //Do left
                                if (m_Up != null)
                                {
                                    if (m_Up.m_Shape.CanSwap())
                                    {
                                        m_Shape.Swap(m_Up.m_Shape, Direction.Up);
                                        GameManager.lastDrag = Direction.Up;
                                    }
                                }
                            }
                            else if (GameManager.lastDrag != Direction.Up)
                                unswap = true;
                        }
                    }

                    //If the last direction we swapped was different, swap back first so we can swap the correct tile
                    if (unswap)
                    {
                        GameManager.dragNItem.Swap(m_Shape, GameManager.GetOpposite(GameManager.lastDrag));
                        GameManager.lastDrag = Direction.None;
                    }
                }
                else
                {
                    //Drag was undone, swap back
                    if (GameManager.lastDrag != Direction.None)
                        GameManager.dragNItem.Swap(m_Shape, GameManager.GetOpposite(GameManager.lastDrag));
                    GameManager.lastDrag = Direction.None;
                }
            }
        }

        if (m_Image != null)
        {
            //FOR TESTING
            //Highlights this node if it can swap into a match
            if (SwapCheckMatch())
                m_Image.color = m_HighlightColour;
            else
                m_Image.color = Color.clear;
        }
    }

    public void MouseDown(BaseEventData eventData)
    {
        //If we can drag, start the drag
        if (!GameManager.CanDrag || !m_Shape.CanSwap())
            return;

        GameManager.isDragging = true;
        GameManager.dragGNode = this;
        GameManager.dragNItem = m_Shape;
        m_Shape.MarkDrag = true;
        GameManager.dragStartPos = Input.mousePosition;
    }

    public void MouseUp(BaseEventData eventData)
    {
        //If the drag was started properly, end the drag
        if (!GameManager.isDragging || GameManager.dragGNode == null)
            return;

        if (GameManager.lastDrag != Direction.None)
        {
            bool ok = false;
            if (!GameManager.dragNItem.m_Parent.CheckMatch(Direction.None, true))
            {
                if (GameManager.dragGNode.CheckMatch(Direction.None, true))
                {
                    ok = true;
                }
            }
            else
                ok = true;

            //If NOT ok, swap the tiles back
            if (!ok)
            {
                GameManager.dragNItem.Swap(GameManager.dragGNode.m_Shape, GameManager.GetOpposite(GameManager.lastDrag));
            }
            else //Otherwise, tell the gamemanager we just moved these two tiles
            {
                GameManager.dragGNode.m_Shape.MarkSwap = true;
                GameManager.dragNItem.MarkSwap = true;
                GameManager.Stationary[GameManager.dragNItem.m_Parent.m_xIndex, GameManager.dragNItem.m_Parent.m_yIndex] = false;
                GameManager.Stationary[GameManager.dragGNode.m_xIndex, GameManager.dragGNode.m_yIndex] = false;
                GameManager.NotifySwap();
            }
        }

        //Finished, reset this data
        GameManager.dragNItem.MarkDrag = false;
        GameManager.dragStartPos = Vector3.zero;
        GameManager.isDragging = false;
        GameManager.dragGNode = null;
        GameManager.dragNItem = null;
        GameManager.lastDrag = Direction.None;
    }

    public void MouseClick(BaseEventData eventData)
    {
        if (GameManager.dragGNode == null && GameManager.CanDrag && m_Shape.CanSwap())
        {
            //First click, to select a tile
            GameManager.dragGNode = this;
            GameManager.dragNItem = m_Shape;
            GameManager.isDragging = false;
            GameManager.dragNItem.MarkDrag = true;
        }
        else if (GameManager.CanDrag)
        {
            //Second click, to swap items
            if (GameManager.dragGNode != this && m_Shape.CanSwap())
            {
                Direction dir = GameManager.dragGNode.TrySwap(this);

                //Only swap them if we can
                if (dir != Direction.None)
                {
                    bool ok = false;
                    if (!GameManager.dragNItem.m_Parent.CheckMatch(Direction.None, true))
                    {
                        if (GameManager.dragGNode.CheckMatch(Direction.None, true))
                        {
                            ok = true;
                            GameManager.dragGNode.m_Shape.MarkSwap = true;
                            GameManager.dragNItem.MarkSwap = true;
                        }
                    }
                    else
                        ok = true;

                    if (!ok)
                    {
                        GameManager.dragNItem.Swap(GameManager.dragGNode.m_Shape, GameManager.GetOpposite(dir));
                    }
                    else
                    {
                        GameManager.dragNItem.MarkSwap = true;
                        GameManager.dragGNode.m_Shape.MarkSwap = true;
                        GameManager.NotifySwap();
                    }
                }
            }

            //Reset this data
            GameManager.dragNItem.MarkDrag = false;
            GameManager.dragGNode = null;
            GameManager.dragNItem = null;
        }
    }

    /// <summary>
    /// Try to swap this node with another and return which way they swapped
    /// </summary>
    /// <param name="a_other"></param>
    /// <returns></returns>
    public Direction TrySwap(GridNode a_other)
    {
        Direction dir = Direction.None;
        if (m_Left == a_other)
            dir = Direction.Left;

        if (m_Right == a_other)
            dir = Direction.Right;

        if (m_Up == a_other)
            dir = Direction.Up;

        if (m_Down == a_other)
            dir = Direction.Down;

        if (dir != Direction.None)
            m_Shape.Swap(a_other.m_Shape, dir);
        return dir;
    }

    /// <summary>
    /// Gives this node a new tile
    /// </summary>
    /// <param name="a_position"></param>
    public void SpawnTile(Vector3 a_position)
    {
        GameObject obj = GameManager.SpawnNodeItem();
        m_Shape = obj.GetComponent<NodeItem>();
        m_Shape.transform.parent = transform.parent;
        m_Shape.transform.localScale = m_ShapeScale;
        m_Shape.transform.position = a_position;
        m_Shape.m_Parent = this;
    }

    public void SpawnTile(Vector3 a_position, GameObject a_object)
    {
        GameObject obj = a_object;
        if (!m_RespawnIsSpawned)
            obj = Instantiate(a_object);
        m_Shape = obj.GetComponent<NodeItem>();
        m_Shape.transform.parent = transform.parent;
        m_Shape.transform.localScale = m_ShapeScale;
        m_Shape.transform.position = a_position;
        m_Shape.m_Parent = this;

        m_RespawnIsSpawned = false;
    }

    /// <summary>
    /// Forcefully take the tile of another node
    /// </summary>
    /// <param name="a_other"></param>
    public void Take(ref GridNode a_other)
    {
        m_Shape = a_other.m_Shape;
        m_Shape.m_Parent = this;
        a_other.m_Shape = null;
    }

    /// <summary>
    /// <para>Try to take the closest upper tile</para>
    /// This is used for dropping the tiles down
    /// </summary>
    public void TryTakeUp()
    {
        GridNode node = null;
        if (m_Up != null)
            node = m_Up.FindNextAvailable();

        //Then takes that tile for itself
        if (node != null)
            Take(ref node);
    }

    /// <summary>
    /// Recursively find the closest node above itself that has a tile
    /// </summary>
    /// <returns></returns>
    public GridNode FindNextAvailable()
    {
        if (m_Shape != null)
            return this;
        if (m_Up != null)
            return m_Up.FindNextAvailable();

        return null;
    }

    /// <summary>
    /// Check for matches. Once it starts in a direction, it will continue that way and not snake around
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="onlyCheck"></param>
    /// <returns></returns>
    public bool CheckMatch(Direction dir = Direction.None, bool onlyCheck = false, ItemColour a_col = ItemColour.NONE)
    {
        if (m_Shape == null)
            return false;

        if (m_Shape.MarkDestroy || !m_Shape.CanDestroy())
            return false;

        if (a_col == ItemColour.NONE)
            a_col = m_Shape.m_Colour;

        //====================================
        //Check each direction

        if (dir == Direction.None || dir == Direction.Left)
        {
            GameManager.NodeChainLeft.Add(this);
            if (m_Left != null)
            {
                if (m_Left.m_Shape != null)
                {
                    if (m_Left.m_Shape.CheckColour(m_Shape))
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
                    if (m_Right.m_Shape.CheckColour(m_Shape))
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
                    if (m_Up.m_Shape.CheckColour(m_Shape))
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
                    if (m_Down.m_Shape.CheckColour(m_Shape))
                    {
                        m_Down.CheckMatch(Direction.Down);
                    }
                }
            }
        }
        //====================================

        if (dir == Direction.None)
        {
            //Only the initiator of the search will get here
            return DestroyCheck(a_col, onlyCheck);
        }
        return false;
    }

    /// <summary>
    /// Checks if we should destroy any tiles (completed matches)
    /// </summary>
    /// <param name="onlyCheck"></param>
    /// <returns></returns>
    bool DestroyCheck(ItemColour a_col, bool onlyCheck = false)
    {
        bool ok = false;
        List<GridNode> destroynodes = new List<GridNode>();
        List<GridNode> leftright = new List<GridNode>();
        List<GridNode> updown = new List<GridNode>();

        //Check the left and add it to a new list
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

        //Is the left/right long enough? Destroy those tiles
        if (leftright.Count > 2)
        {
            ok = true;
            destroynodes.AddRange(leftright);
        }

        //Do the same (as above) for up/down
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
        {
            ok = true;
            destroynodes.AddRange(updown);
        }


        if (!onlyCheck) //Sometimes we only want to check if we can match, but not destroy the tiles
        {
            foreach (var node in destroynodes)
            {
                if (node.m_Shape.MarkSwap)
                {
                    foreach (var otherNode in destroynodes)
                    {
                        otherNode.m_Shape.SwapChain = true;
                    }
                    break;
                }
            }

            if (ok)
            {
                GameManager.NotifyScore(m_Shape.m_Colour, m_Shape.SwapChain, this);
            }

            foreach (var node in destroynodes)
            {
                if (node.m_Shape != null)
                {
                    if (!GameManager.DestroyingList.Contains(node))
                    {
                        //Tell the node to destroy
                        node.m_Shape.m_MatchedColour = a_col;
                        node.StartDestroy();
                    }
                }
            }
        }

        //Clear all those lists
        GameManager.ClearNodeChains();
        return ok;
    }

    public void StartDestroy(bool a_addToRespawn = true)
    {
        GameManager.CanDrag = false;
        if (m_RespawnType == null)
        {
            ++GameManager.RespawnCounts[m_xIndex];
        }
        GameManager.DestroyingList.Add(this);
        m_Shape.MarkDestroy = true;
        m_Shape.StartDestroy();
    }

    public void EndDestroy()
    {
        m_Shape.EndDestroy();
        if (GameManager.DestroyingList.Contains(this))
            GameManager.DestroyingList.Remove(this);

        m_Shape = null;

        if (m_RespawnType != null)
        {
            SpawnTile(transform.position, m_RespawnType);
            m_RespawnType = null;
        }
        GameManager.Stationary[m_xIndex, m_yIndex] = false;
    }

    /// <summary>
    /// Do a sample swap and check if it will result in a match. Used for gameover check
    /// </summary>
    /// <returns></returns>
    public bool SwapCheckMatch()
    {
        if (m_Shape == null)
            return false;
        if (!m_Shape.CanSwap())
            return false;

        if (HasDirection(Direction.Left, true))
        {
            if (m_Shape.CanSwap() && m_Left.m_Shape.CanSwap())
            {
                m_Shape.Swap(m_Left.m_Shape, Direction.Left);
                if (m_Left.CheckMatch(Direction.None, true))
                {
                    m_Shape.Swap(m_Left.m_Shape, Direction.Left);
                    return true;
                }
                m_Shape.Swap(m_Left.m_Shape, Direction.Left);
            }
        }

        if (HasDirection(Direction.Right, true))
        {
            if (m_Shape.CanSwap() && m_Right.m_Shape.CanSwap())
            {
                m_Shape.Swap(m_Right.m_Shape, Direction.Right);
                if (m_Right.CheckMatch(Direction.None, true))
                {
                    m_Shape.Swap(m_Right.m_Shape, Direction.Right);
                    return true;
                }
                m_Shape.Swap(m_Right.m_Shape, Direction.Right);
            }
        }

        if (HasDirection(Direction.Up, true))
        {
            if (m_Shape.CanSwap() && m_Up.m_Shape.CanSwap())
            {
                m_Shape.Swap(m_Up.m_Shape, Direction.Up);
                if (m_Up.CheckMatch(Direction.None, true))
                {
                    m_Shape.Swap(m_Up.m_Shape, Direction.Up);
                    return true;
                }
                m_Shape.Swap(m_Up.m_Shape, Direction.Up);
            }
        }

        if (HasDirection(Direction.Down, true))
        {
            if (m_Shape.CanSwap() && m_Down.m_Shape.CanSwap())
            {
                m_Shape.Swap(m_Down.m_Shape, Direction.Down);
                if (m_Down.CheckMatch(Direction.None, true))
                {
                    m_Shape.Swap(m_Down.m_Shape, Direction.Down);
                    return true;
                }
                m_Shape.Swap(m_Down.m_Shape, Direction.Down);
            }
        }

        return false;
    }

    public bool HasDirection(Direction a_dir, bool a_ShapeInc = false)
    {
        switch (a_dir)
        {
            case Direction.Down:
                {
                    if (m_Down != null)
                    {
                        if (a_ShapeInc)
                        {
                            if (m_Down.m_Shape != null)
                                return true;
                        }
                        else
                            return true;
                    }
                    break;
                }
            case Direction.Up:
                {
                    if (m_Up != null)
                    {
                        if (a_ShapeInc)
                        {
                            if (m_Up.m_Shape != null)
                                return true;
                        }
                        else
                            return true;
                    }
                    break;
                }
            case Direction.Left:
                {
                    if (m_Left != null)
                    {
                        if (a_ShapeInc)
                        {
                            if (m_Left.m_Shape != null)
                                return true;
                        }
                        else
                            return true;
                    }
                    break;
                }
            case Direction.Right:
                {
                    if (m_Right != null)
                    {
                        if (a_ShapeInc)
                        {
                            if (m_Right.m_Shape != null)
                                return true;
                        }
                        else
                            return true;
                    }
                    break;
                }
        }
        return false;
    }
}