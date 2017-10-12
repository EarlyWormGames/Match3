using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTwins : NodeItem
{
    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        GameManager.TopViewTime = 2;
    }
}
