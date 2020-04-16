using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSkillsActiva : ItemInterceptor
{
    public SkillInfo info;

    public void EV_Collect()
    {
        
    }

    protected override bool OnCollect()
    {
        return Main.instance.skillmanager_activas.ReplaceFor(info, 0, myitemworld);
    }
}
