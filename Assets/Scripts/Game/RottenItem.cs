using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenItem : NodeItem
{
    internal bool m_WasDeleted = false;
    public override void OnEndDestroy()
    {
        if (!m_WasDeleted)
            --GameManager.Score;
    }
}
