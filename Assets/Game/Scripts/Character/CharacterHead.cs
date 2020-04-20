using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using DevelopTools;
using Tools.EventClasses;
using Tools.StateMachine;
public class CharacterHead : CharacterControllable
{
    public enum PlayerInputs { IDLE, MOVE, BEGIN_BLOCK, BLOCK, PARRY, CHARGE_ATTACK, RELEASE_ATTACK, TAKE_DAMAGE, DEAD, ROLL };

    Action ChildrensUpdates;
    Func<bool> InDash;

    CharacterBlock charBlock;

    [Header("Dash Options")]
    [SerializeField] float dashTiming;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDeceleration;
    [SerializeField] float dashCD;


    [Header("Movement Options")]
    [SerializeField] float speed;
    [SerializeField] Transform rot;
    CharacterMovement move;

    [Header("Parry & Block Options")]
    [SerializeField] float _timerOfParry;
    [SerializeField] ParticleSystem parryParticle;
    [SerializeField] ParticleSystem hitParticle;
    [SerializeField, Range(-1, 1)] float blockAngle;
    [SerializeField] float parryRecall;
    [SerializeField] float takeDamageRecall;

    //Perdon por esto, pero lo necesito pra la skill del boomeran hasta tener la animacion y el estado "sin escudo"
    bool canBlock = true;
    public GameObject escudo;
    

    [SerializeField] float timeScale;
    [SerializeField] float slowDuration;

    [Header("Feedbacks")]
    public GameObject feedbackParry;
    public GameObject feedbackBlock;
    [SerializeField] ParticleSystem feedbackCW;
    [SerializeField] ParticleSystem feedbackScream;


    [Header("Animations")]
    [SerializeField] Animator anim_base;
    [SerializeField] AnimEvent charAnimEvent;
    CharacterAnimator charanim;

    [Header("Attack Options")]
    [SerializeField] ParticleSystem feedbackHeavy;
    [SerializeField] float dmg_normal = 5;
    [SerializeField] float dmg_heavy = 20;
    [SerializeField] float attackRange;
    [SerializeField] float attackAngle;
    [SerializeField] float timeToHeavyAttack;
    [SerializeField] float rangeOfPetrified;
    [SerializeField] float attackRecall;
    [SerializeField] float onHitRecall;
    float dmg;
    CharacterAttack charAttack;

    CustomCamera customCam;

    public Action ChangeWeaponPassives = delegate { };

    //Modelo del arma para feedback placeholder
    public GameObject currentWeapon;

    [Header("Interactable")]
    public InteractSensor sensor;

    [Header("Life Options")]
    [SerializeField] int life = 100;
    [SerializeField] CharLifeSystem lifesystem;
    public CharLifeSystem Life => lifesystem;

    Rigidbody rb;


    protected override void OnInitialize()
    {
        
    }

    private void Start()
    {
        lifesystem = new CharLifeSystem(life, life)
            .ADD_EVENT_OnGainLife(OnGainLife)
            .ADD_EVENT_OnLoseLife(OnLoseLife)
            .ADD_EVENT_Death(OnDeath)
            .ADD_EVENT_OnChangeValue(OnChangeLife);

        charanim = new CharacterAnimator(anim_base);
        customCam = FindObjectOfType<CustomCamera>();

        move = new CharacterMovement(GetComponent<Rigidbody>(), rot, charanim)
            .SetSpeed(speed)
            .SetTimerDash(dashTiming).SetDashSpeed(dashSpeed)
            .SetDashCD(dashCD)
            .SetRollDeceleration(dashDeceleration);

        InDash += move.IsDash;
        ChildrensUpdates += move.OnUpdate;
        move.SetCallbacks(OnBeginRoll, OnEndRoll);

        charBlock = new CharacterBlock(_timerOfParry, blockAngle, OnEndParry, charanim, feedbackBlock, GetSM);
        charBlock.OnParry += OnBeginParry;
        charBlock.OnParry += charanim.Parry;
        ChildrensUpdates += charBlock.OnUpdate;

        dmg = dmg_normal;
        charAttack = new CharacterAttack(attackRange, attackAngle, timeToHeavyAttack, charanim, rot, ReleaseInNormal, ReleaseInHeavy, feedbackHeavy, rangeOfPetrified, dmg);
        charAttack.FirstAttackReady(true);

        charAnimEvent.Add_Callback("CheckAttackType", CheckAttackType);
        charAnimEvent.Add_Callback("DealAttack", DealAttack);
        charAnimEvent.Add_Callback("RompeCoco", RompeCoco);
        charAnimEvent.Add_Callback("BeginBlock", charBlock.OnBlockSuccessful);

        SetStates();

        rb = GetComponent<Rigidbody>();
    }

    #region SET STATES
    EventStateMachine<PlayerInputs> stateMachine;

    void SetStates()
    {
        var idle = new EState<PlayerInputs>("Idle");
        var move = new EState<PlayerInputs>("Move");
        var beginBlock = new EState<PlayerInputs>("Begin_Block");
        var block = new EState<PlayerInputs>("Block");
        var parry = new EState<PlayerInputs>("Parry");
        var roll = new EState<PlayerInputs>("Roll");
        var attackCharge = new EState<PlayerInputs>("Charge_Attack");
        var attackRelease = new EState<PlayerInputs>("Release_Attack");
        var takeDamage = new EState<PlayerInputs>("Take_Damage");
        var dead = new EState<PlayerInputs>("Dead");

        ConfigureState.Create(idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            .SetTransition(PlayerInputs.PARRY, parry)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(move)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            .SetTransition(PlayerInputs.PARRY, parry)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(beginBlock)
             .SetTransition(PlayerInputs.IDLE, idle)
             .SetTransition(PlayerInputs.MOVE, move)
             .SetTransition(PlayerInputs.BLOCK, block)
             .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
             .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
             .SetTransition(PlayerInputs.DEAD, dead)
             .Done();

        ConfigureState.Create(block)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.MOVE, move)
           // .SetTransition(PlayerInputs.ROLL, roll)
            //.SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(parry)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(roll)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(attackCharge)
            .SetTransition(PlayerInputs.RELEASE_ATTACK, attackRelease)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(attackRelease)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(dead)
            .Done();

        stateMachine = new EventStateMachine<PlayerInputs>(idle);

        new CharIdle(idle, stateMachine).SetLeftAxis(GetLeftHorizontal, GetLeftVertical).SetRightAxis(GetRightHorizontal, GetRightVertical).SetMovement(this.move);
        new CharMove(move, stateMachine).SetLeftAxis(GetLeftHorizontal, GetLeftVertical).SetRightAxis(GetRightHorizontal, GetRightVertical).SetMovement(this.move);
        new CharBeginBlock(beginBlock, stateMachine).SetLeftAxis(GetLeftHorizontal, GetLeftVertical).
            SetRightAxis(GetRightHorizontal, GetRightVertical).SetMovement(this.move).SetBlock(charBlock);
        new CharBlock(block, stateMachine).SetLeftAxis(GetLeftHorizontal, GetLeftVertical).
            SetRightAxis(GetRightHorizontal, GetRightVertical).SetMovement(this.move).SetBlock(charBlock);
        new CharParry(parry, stateMachine, parryRecall).SetMovement(this.move).SetBlock(charBlock);
        new CharRoll(roll, stateMachine).SetMovement(this.move);
        new CharChargeAttack(attackCharge, stateMachine).SetLeftAxis(GetLeftHorizontal, GetLeftVertical).
            SetRightAxis(GetRightHorizontal, GetRightVertical).SetMovement(this.move).SetAttack(charAttack);
        new CharReleaseAttack(attackRelease, stateMachine, attackRecall).SetMovement(this.move).SetAttack(charAttack);
        new CharTakeDmg(takeDamage, stateMachine, takeDamageRecall);
        new CharDead(dead, stateMachine);
    }

    float GetLeftHorizontal() => moveX;
    float GetLeftVertical() => moveY;
    float GetRightHorizontal() => rotateX;
    float GetRightVertical() => rotateY;
    EventStateMachine<PlayerInputs> GetSM() => stateMachine;


    #endregion



    protected override void OnUpdateEntity()
    {
        stateMachine.Update();
        ChildrensUpdates();
        charAttack.Refresh();
    }

    protected override void OnPause()
    {
        
    }
    protected override void OnResume()
    {

    }

    #region Life
    void OnLoseLife() { }
    void OnGainLife() => customCam.BeginShakeCamera();
    void OnDeath() { }
    void OnChangeLife(int current, int max) { Main.instance.GetActivesManager().ReceiveLife(current, max); }
    #endregion

    #region Attack
    /////////////////////////////////////////////////////////////////
    public void EVENT_OnAttackBegin() { stateMachine.SendInput(PlayerInputs.CHARGE_ATTACK); Debug.Log("atroden"); }
    public void EVENT_OnAttackEnd() { stateMachine.SendInput(PlayerInputs.RELEASE_ATTACK); }
    public void CheckAttackType() => charAttack.BeginCheckAttackType();//tengo la espada arriba
    public void DealAttack() => charAttack.OnAttack();
    void ReleaseInNormal()
    {
        dmg = dmg_normal;
        charAttack.ChangeDamageBase((int)dmg);
        charanim.NormalAttack();
    }
    void ReleaseInHeavy()
    {
        dmg = dmg_heavy;
        charAttack.ChangeDamageBase((int)dmg);
        charanim.HeavyAttack();
    }
    ///////////BigWeaponSkill

    /// <summary>
    /// Si manda parametro, es para cmabiar el rango de ataque
    /// </summary>
    /// <param name="newRangeValue"></param>
    /// <returns></returns>
    public float ChangeRangeAttack(float newRangeValue)
    {
        if (newRangeValue < 0)
            return attackRange;
        charAttack.currentWeapon.ModifyAttackrange(newRangeValue);
        return newRangeValue;
    }
    /// <summary>
    /// Si no manda parametros vuelve al rango original del arma
    /// </summary>
    public void ChangeRangeAttack() => charAttack.currentWeapon.ModifyAttackrange();
    public CharacterAttack GetCharacterAttack() => charAttack;
    private void OnDrawGizmos()
    {
        if (charAttack == null)
            return;

        Vector3 attackRange_endPoint =
            transform.position + charAttack.forwardPos.forward * charAttack.currentWeapon.GetWpnRange();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, attackRange_endPoint);
        Gizmos.DrawCube(attackRange_endPoint, new Vector3(.6f, .6f, .6f));
    }

    /////////////////////////////////////////////////////////////////
    #endregion

    #region Block & Parry

    //Toggle con la posibilidad de bloquear o no
    public void ToggleBlock()
    {
        canBlock = !canBlock;
    }

    public void EVENT_OnBlocking()
    {
        //Puesto para no poder bloquear cuando el personaje tira el escudo en el boomeranSkill
        if (!canBlock)
            return;
        
        
        stateMachine.SendInput(PlayerInputs.BEGIN_BLOCK);
    }
    public void EVENT_UpBlocking()
    {
        if (stateMachine.Current.Name == "Block" || stateMachine.Current.Name == "Begin_Block")
        {
            if(moveX == 0 && moveY == 0)
            {
                stateMachine.SendInput(PlayerInputs.IDLE);
            }
            else
            {
                stateMachine.SendInput(PlayerInputs.MOVE);
            }
        }
    }
    
    //lo uso para el skill del escudo que refleja luz
    public EntityBlock GetCharBlock()
    {
        return charBlock;
    }

    public void EVENT_Parry()
    {
        stateMachine.SendInput(PlayerInputs.PARRY);
    }
    public void AddParry(Action listener)
    {
        charBlock.OnParry += listener;
    }
    public void RemoveParry(Action listener)
    {
        charBlock.OnParry -= listener;
    }
    public void PerfectParry()
    {
        parryParticle.Play();
    }

    void OnBeginParry() => feedbackParry.SetActive(true);
    void OnEndParry() => feedbackParry.SetActive(false);

    #endregion

    #region Roll
    void OnBeginRoll()
    {
        
    }

    void OnEndRoll()
    {
        stateMachine.SendInput(PlayerInputs.IDLE);
    }
    public void RollDash()
    {
        if (!move.InCD())
        {
            stateMachine.SendInput(PlayerInputs.ROLL);
        }
    }

    public void AddListenerToDash(Action listener) => move.Dash += listener;
    public void RemoveListenerToDash(Action listener) => move.Dash -= listener;
    public void ChangeDashForTeleport()
    {
        move.Dash -= move.Roll;
        move.Dash += move.Teleport;
    }
    public void ChangeTeleportForDash()
    {
        move.Dash -= move.Teleport;
        move.Dash += move.Roll;
    }
    public CharacterMovement GetCharMove()
    {
        return move;
    }
    void RompeCoco()
    {
        if (customCam != null) customCam.BeginShakeCamera();
    }

    #endregion

    #region Movimiento y Rotacion
    float rotateX;
    float rotateY;
    float moveX;
    float moveY;

    public void LeftHorizontal(float axis)
    {
        moveX = axis;
    }

    public void LeftVerical(float axis)
    {
        moveY = axis;
    }

    public void RightHorizontal(float axis)
    {
        rotateX = axis;
    }
    public void RightVerical(float axis)
    {
        rotateY = axis;
    }
    #endregion

    #region Take Damage
    public override Attack_Result TakeDamage(int dmg, Vector3 attackDir, Damagetype dmgtype)
    {
        if (InDash())
            return Attack_Result.inmune;

        if (charBlock.IsParry(rot.forward, attackDir))
        {
            PerfectParry();
            Main.instance.GetTimeManager().DoSlowMotion(timeScale, slowDuration);
            Debug.Log("Parried");
            return Attack_Result.parried;
        }
        else if (charBlock.IsBlock(rot.forward, attackDir))
        {
            charanim.BlockSomething();
            return Attack_Result.blocked;
        }
        else
        {
            hitParticle.Play();
            lifesystem.Hit(dmg);
            Vector3 dir = (transform.position - attackDir).normalized;
            rb.AddForce(new Vector3(dir.x, 0, dir.z) * dmg * onHitRecall, ForceMode.Force);
            customCam.BeginShakeCamera();
            Main.instance.Vibrate();
            return Attack_Result.sucessful;
        }
    }
    #endregion

    #region Change Weapon

    bool isValue;

    public void ChangeTheWeapon(float w)
    {
        if (!isValue && !charAttack.inAttack)
        {
            if (w == 1 || w == -1)
            {
                charAttack.ChangeWeapon((int)w);
                ChangeWeaponPassives();
                feedbackCW.Stop();
                feedbackCW.Play();
                isValue = true;
            }
        }
        else
        {
            if (w != 1 && w != -1)
            {
                isValue = false;
            }
        }
    }

    public void ChangeDamage(float f)
    {
        charAttack.BuffOrNerfDamage(f);
    }

    #endregion

    #region Interact
    public void UNITY_EVENT_OnInteractDown()
    {
        sensor.OnInteractDown();
    }
    public void UNITY_EVENT_OnInteractUp()
    {
        sensor.OnInteractUp();
    }
    #endregion

    #region Items
    public override void OnReceiveItem(ItemWorld itemworld)
    {
        base.OnReceiveItem(itemworld);
    }
    #endregion

    #region Guilt
    int screams;
    public Action GuiltUltimateSkill = delegate { };
    public Action<int> AddScreamAction = delegate { };
    public int screamsToSkill;

    public void AddScreams(int s)
    {
        screams += s;

        if (screams >= screamsToSkill)
        {
            screams = screamsToSkill;
            GuiltUltimateSkill();
            screams = 0;
        }

        AddScreamAction(screams);
    }

    public void CollectScream()
    {
        AddScreams(1);
        feedbackScream.Stop();
        feedbackScream.Play();
    }

    #endregion

    #region Fuera de uso

    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnFixedUpdate() { }

    

    #endregion

}