using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour : MonoBehaviour {

    private NodeItem m_Shape;
    private Vector3 m_LastPosition;

	void Start ()
    {
        m_LastPosition = transform.position;
        m_Shape = GetComponent<NodeItem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Shape.m_GemAnimator != null)
        {
            if (GameManager.isDragging && GameManager.dragNItem == m_Shape)
            {
                Vector3 dir = Input.mousePosition - GameManager.dragStartPos;
                if (dir.magnitude > 20f)
                {
                    m_Shape.m_GemAnimator.SetFloat("Squish X", dir.x);
                    m_Shape.m_GemAnimator.SetFloat("Squish Y", dir.y);
                    dir.Normalize();
                }
            }
            else if (m_LastPosition != transform.position)
            {
                m_Shape.m_GemAnimator.SetFloat("Squish X", (m_LastPosition.x - transform.position.x) * 100);
                m_Shape.m_GemAnimator.SetFloat("Squish Y", (m_LastPosition.y - transform.position.y) * 100);
                m_LastPosition = transform.position;
            }
            else
            {
                m_Shape.m_GemAnimator.SetFloat("Squish X", 0);
                m_Shape.m_GemAnimator.SetFloat("Squish Y", 0);
            }
        }
	}
}
