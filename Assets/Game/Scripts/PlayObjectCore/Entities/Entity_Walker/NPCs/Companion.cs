using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : NPCBase
{
    public override Attack_Result TakeDamage(int dmg, Vector3 attackDir, Damagetype dmgtype) { return Attack_Result.inmune; }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity() { }
}
