using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkItem : BadNode
{
    public override void OnEndDestroy()
    {
        if (m_bUseScore)
        {
            GameManager.instance.AddScore(-1);
        }
    }
}
