﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : Interactable
{
    public ItemWorld itemToDrop;
    public Transform posToDrop;
    public int QuantityToSpawn;
    public SpawnData data;

    public override void Execute(WalkingEntity collector)
    {
        //if (QuantityToSpawn == 1) Main.instance.SpawnItem(itemToDrop, posToDrop);
        //else Main.instance.SpawnListItems(itemToDrop, posToDrop, QuantityToSpawn);
        Main.instance.SpawnWheel(data, posToDrop);
    }

    public override void Exit()
    {
        WorldItemInfo.instance.Hide();
    }

    public override void ShowInfo(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(posToDrop.position, "Drop items", "Dropea items", "Drop");
    }
}