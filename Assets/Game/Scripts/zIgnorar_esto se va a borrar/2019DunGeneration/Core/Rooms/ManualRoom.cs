using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools.Extensions;
using UnityEngine;

public class ManualRoom : RoomBase
{
    public override void OnInitialize()
    {
        var trigger = GetComponent<RoomTriggers>();
        trigger.Initialize();
        RoomTriggerManager.instancia.dungeonElements.AddListToList(trigger.GetDungeonElements());
        RoomTriggerManager.instancia.SetAll_DungeonGenerationFinallized();

        foreach (var e in elements) e.SetmanualRoom(this);
    }
}
