using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActiva : MonoBehaviour
{
    public SkillInfo info;

    Item myitemworld;

    private void Start()
    {
        myitemworld = GetComponent<ItemWorld>().item;
    }

    public void EV_Collect()
    {
        Main.instance.skillmanager_activas.ReplaceFor(info, 0, myitemworld);
    }
}
