using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkItem : NodeItem
{
    public override void OnEndDestroy()
    {
        --GameManager.Score;
    }
}
