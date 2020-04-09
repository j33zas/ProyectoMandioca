using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamItemWorld : ItemWorld
{
    [HideInInspector]
    public ScreamPool myPool;

    public override void Execute(WalkingEntity collector)
    {
        collector.OnReceiveItem(this);
        to_collect.Invoke();
        myPool.ReturnScream(this);
    }
}
