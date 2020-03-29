using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DummyEnemy : EnemyBase
{
    [SerializeField] CombatComponent combatComponent;
    [SerializeField] int damage;

    public GameObject obj_feedbackStun;
    public GameObject obj_feedbackShield;
    PopSignalFeedback feedbackStun;
    PopSignalFeedback feedbackHitShield;
    public float time_stun;

    void Start()
    {
        combatComponent.Configure(AttackEntity);
        feedbackStun = new PopSignalFeedback(time_stun, obj_feedbackStun, EndStun);
        feedbackHitShield = new PopSignalFeedback(0.2f, obj_feedbackShield);
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
        else if (e.TakeDamage(damage) == Attack_Result.blocked)
        {
            feedbackHitShield.Show();
        }
    }

    private void Update() { feedbackStun.Refresh();  feedbackHitShield.Refresh(); }

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
