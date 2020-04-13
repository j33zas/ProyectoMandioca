using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Minion : Companion
{
    [SerializeField] CombatComponent combatComponent;
    [SerializeField] int damage;

    public GameObject obj_feedbackStun;
    public GameObject obj_feedbackShield;
    public GameObject obj_feedbackattack;
    PopSignalFeedback feedbackStun;
    PopSignalFeedback feedbackHitShield;
    PopSignalFeedback feedbackAttack;

    [SerializeField] ParticleSystem greenblood;

    public float time_stun;

    public AnimEvent anim;
    StatesMachine sm;
    public Animator animator;
    [SerializeField]
    private float _speedMovement;
    [SerializeField]
    private float _rotSpeed;
    [SerializeField]
    private float _petrifiedTime;
    [SerializeField]
    private float _distance;
    //public Follow follow;

    public Rigidbody _rb;

    [Header("Life Options")]
    [SerializeField] GenericLifeSystem lifesystem;

    StatesAttack attackState;
    StatesFollow followState;

    protected override void OnResume()
    {
        _rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        feedbackStun = new PopSignalFeedback(time_stun, obj_feedbackStun, EndStun);
        feedbackHitShield = new PopSignalFeedback(0.2f, obj_feedbackShield);
        feedbackAttack = new PopSignalFeedback(0.2f, obj_feedbackattack);

        anim.Add_Callback("DealDamage", DealDamage);
        sm = new StatesMachine();
        sm.Addstate(new StatesWander(sm));

        followState = new StatesFollow(sm, transform, _rb, FindObjectOfType<TrueDummyEnemy>().transform, animator, _rotSpeed, _distance, _speedMovement);
        sm.Addstate(followState);

        attackState = new StatesAttack(sm, animator, transform, FindObjectOfType<TrueDummyEnemy>().transform, _rotSpeed, _distance);
        sm.Addstate(attackState);

        sm.Addstate(new StatesPetrified(sm, _petrifiedTime));
        
        sm.ChangeState<StatesFollow>();

    }

    protected override void OnUpdateEntity()
    {
        feedbackStun.Refresh(); feedbackHitShield.Refresh(); sm.Update();
    }

    public void ChangeToAttackState(Transform parriedEnemy)
    {
        attackState.ChangeTarget(parriedEnemy);
        followState.ChangeTarget(parriedEnemy);
        sm.ChangeState<StatesFollow>();
    }


    public void DealDamage()
    {
        combatComponent.ManualTriggerAttack();
    }

    public void EndStun() => combatComponent.Play();

    public void AttackEntity(EntityBase e)
    {
        Debug.Log("0: dmg en minion: " + damage);

        if (e.TakeDamage(damage, transform.forward, Damagetype.parriable) == Attack_Result.parried)
        {
            combatComponent.Stop();
            feedbackStun.Show();
        }
        else if (e.TakeDamage(damage, transform.forward, Damagetype.parriable) == Attack_Result.blocked)
        {
            feedbackHitShield.Show();
        }
    }

    /////////////////////////////////////////////////////////////////
    //////  En desuso
    /////////////////////////////////////////////////////////////////
    public override Attack_Result TakeDamage(int dmg, Vector3 dir, Damagetype dmgtype)
    {
        lifesystem.Hit(dmg);
        greenblood.Play();

        return Attack_Result.sucessful;
    }

    public void Petrified()
    {
        sm.ChangeState<StatesPetrified>();
    }

    public float ChangeSpeed(float newSpeed)
    {
        //Si le mando negativo me devuelve la original
        //para guardarla en el componente WebSlowedComponent
        if (newSpeed < 0)
            return _speedMovement;

        //Busco el estado follow para poder cambiarle la velocidad
        StatesFollow statesFollow = sm.GetState<StatesFollow>();
        if (statesFollow != null)
        {
            statesFollow.ChangeSpeed(newSpeed);
        }

        return _speedMovement;
    }


    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }

    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }


}
