using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DummyEnemy : EnemyBase
{
    [SerializeField] CombatComponent combatComponent;
    [SerializeField] int damage;
    void Start() => combatComponent.Configure(AttackEntity);
    public void AttackEntity(EntityBase e) => e.TakeDamage(damage);
    /////////////////////////////////////////////////////////////////
    //////  En desuso
    /////////////////////////////////////////////////////////////////
    public override void TakeDamage(int dmg) { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity() { }
}
