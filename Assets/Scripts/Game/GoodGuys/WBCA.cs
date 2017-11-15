﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCA : NodeItem
{
    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        GameManager.instance.m_WBCATurnsLeft = GameManager.instance.m_WBCATurns;
    }
}
