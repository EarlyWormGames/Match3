using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTwins : GoodNode
{
    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        GameManager.TopViewTime = 3;
    }
}
