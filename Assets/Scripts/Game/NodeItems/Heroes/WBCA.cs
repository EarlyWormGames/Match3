using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBCA : GoodNode
{
    public override void OnEndDestroy()
    {
        base.OnEndDestroy();

        GameManager.instance.ShowWBCA();
    }
}
