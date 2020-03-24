using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXAMPLE_Enemy : EnemyBase
{
    public override void Awake()
    {
        base.Awake(); //no borrar

        /// bla bla bla
    }

    public override void TakeDamage(int dmg) { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity() { }
}
