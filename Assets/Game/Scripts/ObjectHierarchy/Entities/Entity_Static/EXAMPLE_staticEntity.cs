using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXAMPLE_staticEntity : StaticEntity
{
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnFixedUpdate() { }
    protected override void OnUpdate() { }
    public override void TakeDamage(int dmg) { }
}
