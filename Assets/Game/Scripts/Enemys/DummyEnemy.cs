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
    public GameObject obj_feedbackattack;
    public GameObject obj_FeedbackDodge;
    PopSignalFeedback feedbackStun;
    PopSignalFeedback feedbackHitShield;
    PopSignalFeedback feedbackAttack;
    PopSignalFeedback feedbackDodge;

    [SerializeField] ParticleSystem greenblood;
   
    public float time_stun;

    public AnimEvent anim;

    public Follow follow;

    public Rigidbody _rb;

    void Start()
    {
        combatComponent.Configure(AttackEntity);
        feedbackStun = new PopSignalFeedback(time_stun, obj_feedbackStun, EndStun);
        feedbackHitShield = new PopSignalFeedback(0.4f, obj_feedbackShield);
        feedbackAttack = new PopSignalFeedback(0.4f, obj_feedbackattack);
        feedbackDodge = new PopSignalFeedback(0.4f, obj_FeedbackDodge);

        anim.Add_Callback("DealDamage", DealDamage);

        follow.Configure(_rb);
    }

    public void DealDamage()
    {
        combatComponent.ManualTriggerAttack();
    }

    public void EndStun() => combatComponent.Play();

    public void AttackEntity(EntityBase e)
    {
        if (e.TakeDamage(damage) == Attack_Result.parried)
        {
            combatComponent.Stop();
            feedbackStun.Show();
        }
        else if (e.TakeDamage(damage) == Attack_Result.blocked)
        {
            feedbackHitShield.Show();
        }
        else if (e.TakeDamage(damage) == Attack_Result.inmune)
        {
            feedbackDodge.Show();
        }
    }

    private void Update() { feedbackStun.Refresh();  feedbackHitShield.Refresh(); feedbackDodge.Refresh(); }

    /////////////////////////////////////////////////////////////////
    //////  En desuso
    /////////////////////////////////////////////////////////////////
    public override Attack_Result TakeDamage(int dmg)
    {
        greenblood.Play();
        return Attack_Result.sucessful; 
    }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity() { }


}
