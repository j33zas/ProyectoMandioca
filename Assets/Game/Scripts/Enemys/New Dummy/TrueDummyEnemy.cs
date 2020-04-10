using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class TrueDummyEnemy : EnemyBase
{
    [SerializeField] CombatComponent combatComponent;
    [SerializeField] int damage;

    [SerializeField] GameObject obj_feedbackStun;
    [SerializeField] GameObject obj_feedbackShield;
    [SerializeField] GameObject obj_feedbackattack;
    PopSignalFeedback feedbackStun;
    PopSignalFeedback feedbackHitShield;
    PopSignalFeedback feedbackAttack;
    [SerializeField] GameObject feedbackFireDot;

    [SerializeField] ParticleSystem greenblood;

    public bool special;

    public float time_stun;

    public enum DummyEnemyInputs { IDLE, ATTACK, GO_TO_POS, DIE, DISABLE, ENABLE, CHASING, TAKE_DAMAGE, PETRIFIED };

    [SerializeField] AnimEvent anim;
    EventStateMachine<DummyEnemyInputs> sm;
    [SerializeField] Animator animator;
    [SerializeField] float speedMovement;
    [SerializeField] float rotSpeed;
    [SerializeField] float petrifiedTime;
    [SerializeField] float distance;


    public Action OnParried;
    public bool isOnFire { get; private set; }

    public bool isTarget;

    [SerializeField] float explosionForce = 200;
    Rigidbody rb;
    [Header("Life Options")]
    [SerializeField] GenericLifeSystem lifesystem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        feedbackStun = new PopSignalFeedback(time_stun, obj_feedbackStun, EndStun);
        feedbackHitShield = new PopSignalFeedback(0.2f, obj_feedbackShield);
        feedbackAttack = new PopSignalFeedback(0.2f, obj_feedbackattack);

        anim.Add_Callback("DealDamage", DealDamage);

        lifesystem.AddEventOnDeath(Die);
        currentSpeed = speedMovement;
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
            if (OnParried != null)
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



    /////////////////////////////////////////////////////////////////
    //////  En desuso
    /////////////////////////////////////////////////////////////////
    public override Attack_Result TakeDamage(int dmg, Vector3 dir, Damagetype dmgtype)
    {
        if (Invinsible)
            return Attack_Result.inmune;
        if (dmgtype == Damagetype.explosion)
        {
            rb.AddForce(dir * explosionForce, ForceMode.Impulse);
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
        if (!target)
            Invinsible = true;
    }
    public override void OnPetrified()
    {
        feedbackStun.Show();
        base.OnPetrified();
        sm.SendInput(DummyEnemyInputs.PETRIFIED);
    }

    public float ChangeSpeed(float newSpeed)
    {
        //Si le mando negativo me devuelve la original
        //para guardarla en el componente WebSlowedComponent
        if (newSpeed < 0)
            return speedMovement;

        //Busco el estado follow para poder cambiarle la velocidad
        currentSpeed = newSpeed;


        return speedMovement;
    }

    public void getFocusedOnParry()
    {
        foreach (var item in Main.instance.GetEnemies())
        {
            if (item != this)
                item.isTarget = false;
            else
                isTarget = true;
        }
    }


    public override void OnFire()
    {
        if (isOnFire)
            return;

        isOnFire = true;
        feedbackFireDot.SetActive(true);
        base.OnFire();

        lifesystem.DoTSystem(30, 2, 1, Damagetype.Fire, () =>
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

    #region STATE MACHINE THINGS

    void SetStates()
    {
        var idle = new EState<DummyEnemyInputs>();
        var goToPos = new EState<DummyEnemyInputs>();
        var chasing = new EState<DummyEnemyInputs>();
        var attack = new EState<DummyEnemyInputs>();
        var takeDamage = new EState<DummyEnemyInputs>();
        var die = new EState<DummyEnemyInputs>();
        var disable = new EState<DummyEnemyInputs>();
        var petrified = new EState<DummyEnemyInputs>();

        ConfigureState.Create(idle)
            .SetTransition(DummyEnemyInputs.GO_TO_POS, goToPos)
            .SetTransition(DummyEnemyInputs.CHASING, chasing)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.CHASING, chasing)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(chasing)
            .SetTransition(DummyEnemyInputs.ATTACK, attack)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(attack)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.CHASING, chasing)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .Done();

        ConfigureState.Create(die)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .Done();

        

        ConfigureState.Create(takeDamage)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.CHASING, chasing)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .Done();

        //Asignando las funciones de cada estado
        CreateIdle(idle);
        CreateGoTo(goToPos);
        CreateChasing(chasing);
        CreateTakeDamage(takeDamage);
        CreatePetrified(petrified);
        CreateDie(die);
        CreateDisable(disable);
        /////////////////////////////////////////////////


        sm = new EventStateMachine<DummyEnemyInputs>(idle);

    }

    #region STATES
    float currentSpeed;


    void CreateIdle(EState<DummyEnemyInputs> idle)
    {
        //idle.OnEnter +=;

        //idle.OnUpdate +=;

        //idle.OnLateUpdate +=;

        //idle.OnFixedUpdate +=;

        //idle.OnExit +=;
    }

    void CreateGoTo(EState<DummyEnemyInputs> goTo)
    {
        //idle.OnEnter +=;

        //idle.OnUpdate +=;

        //idle.OnLateUpdate +=;

        //idle.OnFixedUpdate +=;

        //idle.OnExit +=;
    }

    void CreateChasing(EState<DummyEnemyInputs> chasing)
    {
        //idle.OnEnter +=;

        //idle.OnUpdate +=;

        //idle.OnLateUpdate +=;

        //idle.OnFixedUpdate +=;

        //idle.OnExit +=;
    }

    void CreateAttack(EState<DummyEnemyInputs> attack)
    {
        //idle.OnEnter +=;

        //idle.OnUpdate +=;

        //idle.OnLateUpdate +=;

        //idle.OnFixedUpdate +=;

        //idle.OnExit +=;
    }

    void CreateTakeDamage(EState<DummyEnemyInputs> takeDmg)
    {
        //idle.OnEnter +=;

        //idle.OnUpdate +=;

        //idle.OnLateUpdate +=;

        //idle.OnFixedUpdate +=;

        //idle.OnExit +=;
    }

    void CreatePetrified(EState<DummyEnemyInputs> petrified)
    {
        //idle.OnEnter +=;

        //idle.OnUpdate +=;

        //idle.OnLateUpdate +=;

        //idle.OnFixedUpdate +=;

        //idle.OnExit +=;
    }

    void CreateDie(EState<DummyEnemyInputs> die)
    {
        //idle.OnEnter +=;

        //idle.OnUpdate +=;

        //idle.OnLateUpdate +=;

        //idle.OnFixedUpdate +=;

        //idle.OnExit +=;
    }

    void CreateDisable(EState<DummyEnemyInputs> disable)
    {
        //idle.OnEnter +=;

        //idle.OnUpdate +=;

        //idle.OnLateUpdate +=;

        //idle.OnFixedUpdate +=;

        //idle.OnExit +=;
    }

    #endregion



    #endregion

}
