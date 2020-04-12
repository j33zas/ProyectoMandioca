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
    PopSignalFeedback feedbackStun;
    PopSignalFeedback feedbackHitShield;
    PopSignalFeedback feedbackAttack;
    [SerializeField] GameObject feedbackFireDot;

    [SerializeField] ParticleSystem greenblood;

    public bool special;
   
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
    private float _frozenTime;
    [SerializeField]
    private float _distance;
    //public Follow follow;
    public bool isOnFire { get; private set; }

    public float explosionForce = 200;
    public Rigidbody _rb;
    [SerializeField]
    private bool off;
    [Header("Life Options")]
    [SerializeField] GenericLifeSystem lifesystem;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        feedbackStun = new PopSignalFeedback(time_stun, obj_feedbackStun, EndStun);
        feedbackHitShield = new PopSignalFeedback(0.2f, obj_feedbackShield);
        feedbackAttack = new PopSignalFeedback(0.2f, obj_feedbackattack);

        anim.Add_Callback("DealDamage", DealDamage);
        sm = new StatesMachine();
        sm.Addstate(new StatesFollow(sm, transform, _rb, FindObjectOfType<CharacterHead>().transform, animator, _rotSpeed, _distance, _speedMovement));
        sm.Addstate(new StatesAttack(sm, animator, transform, FindObjectOfType<CharacterHead>().transform, _rotSpeed, _distance));
        sm.Addstate(new StatesPetrified(sm, _petrifiedTime));
        sm.Addstate(new FreezeState(sm, _frozenTime));
        sm.ChangeState<StatesAttack>();
        
        lifesystem.AddEventOnDeath(Die);


        //follow.Configure(_rb);
        if (off)
            Off();
    }

    public void DealDamage()
    {
        combatComponent.ManualTriggerAttack();
    }

    public void EndStun()
    {
        combatComponent.Play();
        petrified = false;
    }

    public void AttackEntity(EntityBase e)
    {

        if (e.TakeDamage(damage, transform.forward, Damagetype.parriable) == Attack_Result.parried)
        {
            combatComponent.Stop();
            feedbackStun.Show();

            //Tira evento si es parrieado. Seguro haya que cambiarlo
            if(OnParried != null)
                OnParried();
        }
        else if (e.TakeDamage(damage, transform.forward, Damagetype.parriable) == Attack_Result.blocked)
        {
            feedbackHitShield.Show();
        }
    }



    protected override void OnUpdateEntity() 
    {
        if (canupdate)
        {
            feedbackStun.Refresh();
            feedbackHitShield.Refresh();
            sm.Update();
        }
    }
    protected override void OnPause() 
    {
        
    }
    protected override void OnResume() 
    {
        
       
    }


    //No sabia muy bien donde ponerlo, asi que lo pongo aca. Esto lo llama el freeze_range active skill

    public override void OnFreeze()
    {
        base.OnFreeze();
        sm.ChangeState<FreezeState>();
        
    }

    /////////////////////////////////////////////////////////////////
    //////  En desuso
    /////////////////////////////////////////////////////////////////
    public override Attack_Result TakeDamage(int dmg, Vector3 dir,  Damagetype dmgtype)
    {
        if (Invinsible)
            return Attack_Result.inmune;
        if (dmgtype == Damagetype.explosion)
        {
            _rb.AddForce(dir * explosionForce, ForceMode.Impulse);
        }


        Debug.Log("Attack result: " + dmg);
        lifesystem.Hit(dmg);
        greenblood.Play();

        return Attack_Result.sucessful; 
    }
    public override void HalfLife()
    {
        base.HalfLife();
        TakeDamage(lifesystem.life / 2, transform.position, Damagetype.normal);
        if (!base.target)
            Invinsible = true;
    }
    public override void OnPetrified()
    {
        feedbackStun.Show();
        base.OnPetrified();
        sm.ChangeState<StatesPetrified>();
    }

    public override float ChangeSpeed(float newSpeed)
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
    
    public override void OnFire()
    {
        if (isOnFire)
            return;

        isOnFire = true;
        feedbackFireDot.SetActive(true);
        base.OnFire();
        
        lifesystem.DoTSystem(30,2,1,Damagetype.Fire, () =>
        {
            isOnFire = false;
            feedbackFireDot.SetActive(false);
        });
        
    }
    public void Die()
    {
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_DEAD, new object[] { transform.position, petrified });
        gameObject.SetActive(false);
    }
    protected override void OnFixedUpdate() { }
    
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }

    public override void ToAttack()
    {
        
    }

    public override void IAInitialize(CombatDirector _director)
    {

    }
}
