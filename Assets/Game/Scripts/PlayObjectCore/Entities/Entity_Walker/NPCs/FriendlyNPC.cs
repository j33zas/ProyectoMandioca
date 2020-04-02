using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyNPC : NPCBase
{
    

    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity() { }
    public override Attack_Result TakeDamage(int dmg, Vector3 attackDir) { return Attack_Result.inmune; }
}
