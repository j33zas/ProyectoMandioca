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
    [SerializeField] CombatDirector director;
    [SerializeField] Transform rootTransform;

    public enum DummyEnemyInputs { IDLE, ATTACK, GO_TO_POS, DIE, DISABLE, TAKE_DAMAGE, PETRIFIED };

    [SerializeField] AnimEvent anim;
    EventStateMachine<DummyEnemyInputs> sm;
    [SerializeField] Animator animator;
    [SerializeField] float speedMovement;
    [SerializeField] float rotSpeed;
    [SerializeField] float petrifiedTime;
    [SerializeField] float distanceToAttack;
    [SerializeField] float normalDistance;

    public bool isOnFire { get; private set; }

    [SerializeField] float explosionForce = 200;
    Rigidbody rb;
    [Header("Life Options")]
    [SerializeField] GenericLifeSystem lifesystem;

    protected override void OnInitialize()
    {
        rb = GetComponent<Rigidbody>();
        combatComponent.Configure(AttackEntity);
        feedbackStun = new PopSignalFeedback(time_stun, obj_feedbackStun, EndStun);
        feedbackHitShield = new PopSignalFeedback(0.2f, obj_feedbackShield);
        feedbackAttack = new PopSignalFeedback(0.2f, obj_feedbackattack);
        anim.Add_Callback("DealDamage", DealDamage);
        lifesystem.AddEventOnDeath(Die);
        currentSpeed = speedMovement;

        SetTarget(Main.instance.GetChar());
        IAInitialize(Main.instance.GetCombatDirector());
        sm.SendInput(DummyEnemyInputs.ATTACK);

    }

    public override void PlayerEnterRoom()
    {
        SetTarget(Main.instance.GetChar());
        IAInitialize(Main.instance.GetCombatDirector());
    }

    public override void PlayerLeaveRoom()
    {
        sm.SendInput(DummyEnemyInputs.DISABLE);
    }

    public override void IAInitialize(CombatDirector _director)
    {
        director = _director;
        if (sm == null)
            SetStates();
        else
        {
            sm.SendInput(DummyEnemyInputs.IDLE);
        }

        canupdate = true;
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
        if (e.TakeDamage(damage, transform.position, Damagetype.parriable) == Attack_Result.parried)
        {
            combatComponent.Stop();
            feedbackStun.Show();

            //Tira evento si es parrieado. Seguro haya que cambiarlo
            if (OnParried != null)
                OnParried();
        }
        else if (e.TakeDamage(damage, transform.position, Damagetype.parriable) == Attack_Result.blocked)
        {
            feedbackHitShield.Show();
        }
    }

    protected override void OnUpdateEntity()
    {
        if (canupdate)
        {

            if(!combat && entityTarget == null)
            {
                if (Vector3.Distance(Main.instance.GetChar().transform.position, transform.position) <= combatDistance)
                {
                    director.AddAwake(this);
                    combat = true;
                }
            }


            feedbackStun.Refresh();
            feedbackHitShield.Refresh();
            if (sm != null)
            {
                sm.Update();
            }
        }
    }
    protected override void OnPause()
    {

    }
    protected override void OnResume()
    {


    }

    public override Attack_Result TakeDamage(int dmg, Vector3 dir, Damagetype dmgtype)
    {
        if (Invinsible)
            return Attack_Result.inmune;
        if (dmgtype == Damagetype.explosion)
        {
            rb.AddForce(dir * explosionForce, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(dir * forceRecall, ForceMode.Impulse);
        }

        sm.SendInput(DummyEnemyInputs.TAKE_DAMAGE);

        greenblood.Play();

        lifesystem.Hit(dmg);

        return Attack_Result.sucessful;
    }

    public override Attack_Result TakeDamage(int dmg, Vector3 attackDir, Damagetype damagetype, EntityBase owner_entity)
    {
        if (sm.Current.Name != "Attack" && entityTarget != owner_entity)
        {
            attacking = false;
            if (entityTarget == null) throw new System.Exception("entity target es null");//esto rompe cuando vengo desde el Damage in Room
            director.RemoveToAttack(this, entityTarget);
            SetTarget(owner_entity);
            director.AddToAttack(this, entityTarget);
        }

        return TakeDamage(dmg, attackDir, damagetype);
    }

    public override void HalfLife()
    {
        base.HalfLife();
        TakeDamage(lifesystem.life / 2, transform.position, Damagetype.normal);
        if (!base.target)
            Invinsible = true;
    }
    public override void InstaKill()
    {
        base.InstaKill();
        TakeDamage(lifesystem.life, transform.position, Damagetype.normal);
    }
    public override void OnPetrified()
    {
        feedbackStun.Show();
        base.OnPetrified();
        sm.SendInput(DummyEnemyInputs.PETRIFIED);
    }

    public override float ChangeSpeed(float newSpeed)
    {
        //Si le mando negativo me devuelve la original
        //para guardarla en el componente WebSlowedComponent
        if (newSpeed < 0)
            return speedMovement;

        //Busco el estado follow para poder cambiarle la velocidad
        currentSpeed = newSpeed;


        return speedMovement;
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
        director.RemoveToAttack(this, entityTarget);
        Main.instance.eventManager.TriggerEvent(GameEvents.ENEMY_DEAD, new object[] { transform.position, petrified });
        gameObject.SetActive(false);
        //sm.SendInput(DummyEnemyInputs.DIE);
    }
    protected override void OnFixedUpdate() { }

    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }

    public override void ToAttack()
    {
        attacking = true;
    }

    #region STATE MACHINE THINGS

    void SetStates()
    {
        var idle = new EState<DummyEnemyInputs>("Idle");
        var goToPos = new EState<DummyEnemyInputs>("Follow");
        var attack = new EState<DummyEnemyInputs>("Attack");
        var takeDamage = new EState<DummyEnemyInputs>("Take_Damage");
        var die = new EState<DummyEnemyInputs>("Die");
        var disable = new EState<DummyEnemyInputs>("Disable");
        var petrified = new EState<DummyEnemyInputs>("Petrified");

        ConfigureState.Create(idle)
            .SetTransition(DummyEnemyInputs.GO_TO_POS, goToPos)
            .SetTransition(DummyEnemyInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(DummyEnemyInputs.ATTACK, attack)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        ConfigureState.Create(goToPos)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
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
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .SetTransition(DummyEnemyInputs.PETRIFIED, petrified)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .Done();

        ConfigureState.Create(die)
            .Done();

        ConfigureState.Create(disable)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .Done();

        ConfigureState.Create(petrified)
            .SetTransition(DummyEnemyInputs.IDLE, idle)
            .SetTransition(DummyEnemyInputs.ATTACK, attack)
            .SetTransition(DummyEnemyInputs.DIE, die)
            .SetTransition(DummyEnemyInputs.DISABLE, disable)
            .Done();

        sm = new EventStateMachine<DummyEnemyInputs>(idle);

        var head = Main.instance.GetChar();

        //Asignando las funciones de cada estado
        new DummyIdleState(idle, sm, IsAttack, CurrentTargetPos, distanceToAttack, normalDistance,this).SetAnimator(animator).SetRoot(rootTransform)
                                                                                                                     .SetDirector(director);

        new DummyFollowState(goToPos, sm, avoidRadious, avoidWeight, GetCurrentSpeed, CurrentTargetPos, normalDistance, this).SetAnimator(animator).SetRigidbody(rb)
                                                                                                          .SetRoot(rootTransform);

        new DummyAttackState(attack, sm, cdToAttack, this).SetAnimator(animator).SetDirector(director);

        new DummyTDState(takeDamage, sm, recallTime).SetAnimator(animator);

        new DummyStunState(petrified, sm, petrifiedTime, attack).SetAnimator(animator);

        new DummyDieState(die, sm).SetAnimator(animator).SetDirector(director).SetRigidbody(rb);

        new DummyDisableState(disable, sm, EnableObject, DisableObject);

        /////////////////////////////////////////////////
        ///

    }

    float currentSpeed;
    [SerializeField] float avoidWeight;
    [SerializeField] float avoidRadious;
    [SerializeField] float cdToAttack;
    [SerializeField] float recallTime;
    [SerializeField] float forceRecall;

    float GetCurrentSpeed() { return currentSpeed; }

    void DisableObject()
    {
        canupdate = false;
        currentSpeed = speedMovement;
        combat = false;
    }

    void EnableObject() { Initialize(); }

   

    #endregion

}
