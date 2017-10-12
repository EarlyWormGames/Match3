using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenItem : NodeItem
{
    public override void OnEndDestroy()
    {
        if (m_bUseScore)
            --GameManager.Score;
    }
}
