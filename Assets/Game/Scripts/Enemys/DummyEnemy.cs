using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DummyEnemy : EnemyBase
{
    [SerializeField] CombatComponent combatComponent;
    [SerializeField] int damage;

    public GameObject obj_feedbackStun;
    PopSignalFeedback feedbackStun;
    public float time_stun;

    void Start()
    {
        combatComponent.Configure(AttackEntity);
        feedbackStun = new PopSignalFeedback(time_stun, obj_feedbackStun, EndStun);
    }

    public void EndStun() => combatComponent.Play();

    public void AttackEntity(EntityBase e)
    {
        if (e.TakeDamage(damage) == Attack_Result.parried)
        {
            combatComponent.Stop();
            feedbackStun.Show();
            Debug.Log("PARRIED");
        }
    }

    private void Update() => feedbackStun.Refresh();

    /////////////////////////////////////////////////////////////////
    //////  En desuso
    /////////////////////////////////////////////////////////////////
    public override Attack_Result TakeDamage(int dmg) { return Attack_Result.inmune; }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity() { }


}
