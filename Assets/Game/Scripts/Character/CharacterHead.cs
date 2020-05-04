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
    public enum PlayerInputs { IDLE, MOVE, BEGIN_BLOCK, BLOCK, END_BLOCK, PARRY, CHARGE_ATTACK, RELEASE_ATTACK, TAKE_DAMAGE, DEAD, ROLL, SPIN, STUN };

    Action ChildrensUpdates;

    [Header("Dash Options")]
    [SerializeField] float dashTiming = 2;
    [SerializeField] float dashSpeed = 9;
    [SerializeField] float dashDeceleration = 5;
    [SerializeField] float dashCD = 2;
    [SerializeField] ParticleSystem evadeParticle = null;
    Func<bool> InDash;

    [Header("Movement Options")]
    [SerializeField] float speed = 5;
    [SerializeField] Transform rot = null;
    CharacterMovement move;

    [Header("Parry & Block Options")]
    [SerializeField] float _timerOfParry = 1;
    [SerializeField] float _timeOfBlock = 3;
    [SerializeField] int maxBlockCharges = 3;
    [SerializeField] float timeToRecuperateCharges = 5;
    [SerializeField] GameObject chargesUI = null;
    [SerializeField] ParticleSystem parryParticle = null;
    [SerializeField] ParticleSystem blockParticle = null;
    [SerializeField] ParticleSystem hitParticle = null;
    [SerializeField, Range(-1, 1)] float blockAngle = 0;
    [SerializeField] float parryRecall = 0;
    [SerializeField] float takeDamageRecall = 0;
    CharacterBlock charBlock;

    //Perdon por esto, pero lo necesito pra la skill del boomeran hasta tener la animacion y el estado "sin escudo"
    bool canBlock = true;
    public GameObject escudo;

    [Header("SlowMotion")]
    [SerializeField] float timeScale = 1;
    [SerializeField] float slowDuration = 2;
    [SerializeField] float speedAnim = 1;

    [Header("Feedbacks")]
    [SerializeField] ParticleSystem feedbackCW = null;
    [SerializeField] ParticleSystem feedbackScream = null;
    [SerializeField] ParticleSystem inParryParticles = null;

    [Header("Animations")]
    [SerializeField] Animator anim_base = null;
    [SerializeField] AnimEvent charAnimEvent = null;
    CharacterAnimator charanim;

    [Header("Attack Options")]
    [SerializeField] ParticleSystem feedbackHeavy = null;
    [SerializeField] float dmg_normal = 5;
    [SerializeField] float dmg_heavy = 20;
    [SerializeField] float attackRange = 3;
    [SerializeField] float attackAngle = 90;
    [SerializeField] float timeToHeavyAttack = 1.5f;
    [SerializeField] float rangeOfPetrified = 5;
    [SerializeField] float attackRecall = 1;
    [SerializeField] float onHitRecall = 2;
    [SerializeField] Sensor sensorSpin = null;
    float dmg;
    CharacterAttack charAttack;
    [SerializeField] ParticleSystem slash = null;

    CustomCamera customCam;

    [SerializeField] GameObject go_StunFeedback = null;
    [SerializeField] GameObject go_SpinFeedback = null;
    float spinDuration;
    float spinSpeed;
    float stunDuration;

    public Action ChangeWeaponPassives = delegate { };

    //Modelo del arma para feedback placeholder
    public GameObject currentWeapon;

    [Header("Interactable")]
    public InteractSensor sensor;

    [Header("Life Options")]
    [SerializeField] int life = 100;
    [SerializeField] CharLifeSystem lifesystem = null;
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

        charBlock = new CharacterBlock(_timerOfParry, blockAngle, _timeOfBlock, maxBlockCharges, timeToRecuperateCharges, chargesUI, charanim, GetSM, inParryParticles);
        charBlock.OnParry += () => charanim.Parry(true);
        charBlock.EndBlock += EVENT_UpBlocking;
        ChildrensUpdates += charBlock.OnUpdate;

        dmg = dmg_normal;
        charAttack = new CharacterAttack(attackRange, attackAngle, timeToHeavyAttack, charanim, rot, ReleaseInNormal, ReleaseInHeavy, feedbackHeavy, dmg, slash);
        charAttack.FirstAttackReady(true);

        charAnimEvent.Add_Callback("CheckAttackType", CheckAttackType);
        charAnimEvent.Add_Callback("DealAttack", DealAttack);
        charAnimEvent.Add_Callback("EndAttack", EndAttack);
        charAnimEvent.Add_Callback("RompeCoco", RompeCoco);
        charAnimEvent.Add_Callback("BeginBlock", charBlock.OnBlockSuccessful);
        charAnimEvent.Add_Callback("Dash", move.RollForAnim);

        SetStates();

        rb = GetComponent<Rigidbody>();

        StartDebug();
    }

    #region SET STATES
    EventStateMachine<PlayerInputs> stateMachine;

    void SetStates()
    {
        var idle = new EState<PlayerInputs>("Idle");
        var move = new EState<PlayerInputs>("Move");
        var beginBlock = new EState<PlayerInputs>("Begin_Block");
        var block = new EState<PlayerInputs>("Block");
        var endBlock = new EState<PlayerInputs>("End_Block");
        var parry = new EState<PlayerInputs>("Parry");
        var roll = new EState<PlayerInputs>("Roll");
        var attackCharge = new EState<PlayerInputs>("Charge_Attack");
        var attackRelease = new EState<PlayerInputs>("Release_Attack");
        var takeDamage = new EState<PlayerInputs>("Take_Damage");
        var dead = new EState<PlayerInputs>("Dead");
        var spin = new EState<PlayerInputs>("Spin");
        var stun = new EState<PlayerInputs>("Stun");

        ConfigureState.Create(idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            //.SetTransition(PlayerInputs.PARRY, parry)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.SPIN, spin)
            .SetTransition(PlayerInputs.STUN, stun)
            .Done();

        ConfigureState.Create(move)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.BEGIN_BLOCK, beginBlock)
            //.SetTransition(PlayerInputs.PARRY, parry)
            .SetTransition(PlayerInputs.ROLL, roll)
            .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.DEAD, dead)
            .SetTransition(PlayerInputs.SPIN, spin)
            .SetTransition(PlayerInputs.STUN, stun)
            .Done();

        ConfigureState.Create(beginBlock)
             .SetTransition(PlayerInputs.BLOCK, block)
             .SetTransition(PlayerInputs.END_BLOCK, endBlock)
             .SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
             .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
             .SetTransition(PlayerInputs.DEAD, dead)
             .Done();

        ConfigureState.Create(block)
             .SetTransition(PlayerInputs.END_BLOCK, endBlock)
            // .SetTransition(PlayerInputs.ROLL, roll)
            //.SetTransition(PlayerInputs.CHARGE_ATTACK, attackCharge)
            .SetTransition(PlayerInputs.TAKE_DAMAGE, takeDamage)
            .SetTransition(PlayerInputs.PARRY, parry)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(endBlock)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.MOVE, move)
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
            .SetTransition(PlayerInputs.CHARGE_ATTACK, idle)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(takeDamage)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.DEAD, dead)
            .Done();

        ConfigureState.Create(spin)
            .SetTransition(PlayerInputs.STUN, stun)
            .Done();

        ConfigureState.Create(stun)
            .SetTransition(PlayerInputs.IDLE, idle)
            .SetTransition(PlayerInputs.MOVE, move)
            .Done();

        ConfigureState.Create(dead)
            .Done();

        stateMachine = new EventStateMachine<PlayerInputs>(idle, DebugState);

        new CharIdle(idle, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetRightAxis(GetRightHorizontal, GetRightVertical)
            .SetMovement(this.move);

        new CharMove(move, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetRightAxis(GetRightHorizontal, GetRightVertical)
            .SetMovement(this.move);

        new CharBeginBlock(beginBlock, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetRightAxis(GetRightHorizontal, GetRightVertical)
            .SetMovement(this.move).SetBlock(charBlock);

        new CharBlock(block, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetRightAxis(GetRightHorizontal, GetRightVertical)
            .SetMovement(this.move)
            .SetBlock(charBlock);

        new CharEndBlock(endBlock, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetBlock(charBlock);

        new CharParry(parry, stateMachine, parryRecall)
            .SetMovement(this.move).SetBlock(charBlock);

        new CharRoll(roll, stateMachine, evadeParticle)
            .SetMovement(this.move);

        new CharChargeAttack(attackCharge, stateMachine)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            .SetRightAxis(GetRightHorizontal, GetRightVertical)
            .SetMovement(this.move)
            .SetAttack(charAttack);

        new CharReleaseAttack(attackRelease, stateMachine, attackRecall, HeavyAttackRealease, ChangeHeavy)
            .SetMovement(this.move)
            .SetAttack(charAttack)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical);

        new CharTakeDmg(takeDamage, stateMachine, takeDamageRecall);

        new CharSpin(spin, stateMachine)
            .Configurate(GetSpinDuration, GetSpinSpeed, go_SpinFeedback, sensorSpin)
            .SetLeftAxis(GetLeftHorizontal, GetLeftVertical)
            //.SetRightAxis(GetRightHorizontal, GetRightVertical)
            .SetMovement(this.move)
            .SetAnimator(charanim);

        new CharStun(stun, stateMachine)
            .Configurate(GetStunDuration, go_StunFeedback)
            .SetAnimator(charanim);

        new CharDead(dead, stateMachine);
    }

    float GetLeftHorizontal() => moveX;
    float GetLeftVertical() => moveY;
    float GetRightHorizontal() => rotateX;
    float GetRightVertical() => rotateY;
    float GetSpinDuration() => spinDuration;
    float GetSpinSpeed() => spinSpeed;
    float GetStunDuration() => stunDuration;
    EventStateMachine<PlayerInputs> GetSM() => stateMachine;


    #endregion

    public void StartSpin(float _spinDuration, float _spinSpeed, float _stunDuration)
    {
        spinDuration = _spinDuration;
        spinSpeed = _spinSpeed;
        stunDuration = _stunDuration;
        stateMachine.SendInput(PlayerInputs.SPIN);
    }

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
    public void EVENT_OnAttackBegin() { stateMachine.SendInput(PlayerInputs.CHARGE_ATTACK); }
    public void EVENT_OnAttackEnd() { stateMachine.SendInput(PlayerInputs.RELEASE_ATTACK); }
    public void CheckAttackType() => charAttack.BeginCheckAttackType();//tengo la espada arriba
    public void DealAttack()
    {
        charAttack.ConfigureDealsSuscessful(DealSucessfullNormal, DealSucessfullHeavy, KillInNormal, KillInHeavy, BreakObject);
        charAttack.OnAttack(isHeavyRelease);
    }

    #region Resultados de los ataques
    void DealSucessfullNormal()
    {
        //Main.instance.GetTimeManager().DoHitStop();
        Main.instance.Vibrate(0.7f, 0.1f);
        Main.instance.CameraShake();
    }
    void DealSucessfullHeavy()
    {
        ChangeHeavy(false);
        Main.instance.Vibrate(1f, 0.2f);
        Main.instance.CameraShake();
    }
    void KillInNormal()
    {
        Main.instance.GetTimeManager().DoHitStop();
        Main.instance.Vibrate(0.7f, 0.1f);
        Main.instance.CameraShake();
    }
    void KillInHeavy()
    {
        SlowMO();
        ChangeHeavy(false);
        Main.instance.Vibrate(1f, 0.5f);
        Main.instance.CameraShake();
    }
    void BreakObject()
    {
        Main.instance.CameraShake();
    }
    #endregion
    void ReleaseInNormal()
    {
        ChangeDamageAttack((int)dmg_normal);
        ChangeAngleAttack(attackAngle);
        ChangeRangeAttack(attackRange);
        charanim.NormalAttack();
    }
    bool isHeavyRelease;
    void ReleaseInHeavy()
    {
        ChangeHeavy(true);
        ChangeDamageAttack((int)dmg_heavy);
        ChangeAngleAttack(attackAngle * 2);
        ChangeRangeAttack(attackRange + 1);
        charanim.HeavyAttack();
    }
    void ChangeHeavy(bool y) { isHeavyRelease = y; }

    bool HeavyAttackRealease() { return isHeavyRelease; }

    void EndAttack()
    {
        stateMachine.SendInput(PlayerInputs.IDLE);
    }

    ///////////BigWeaponSkill

    public void ChangeDamageAttack(int newDamageValue) => charAttack.ChangeDamageBase(newDamageValue);
    public float ChangeRangeAttack(float newRangeValue = -1) => charAttack.currentWeapon.ModifyAttackrange(newRangeValue);
    public float ChangeAngleAttack(float newAngleValue = -1) => charAttack.currentWeapon.ModifyAttackAngle(newAngleValue);
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
    public void ToggleBlock(bool val)
    {
        canBlock = val;
    }

    public void EVENT_OnBlocking()
    {
        //Puesto para no poder bloquear cuando el personaje tira el escudo en el boomeranSkill
        if (!canBlock)
            return;

        if (charBlock.CurrentBlockCharges > 0)
        {
            stateMachine.SendInput(PlayerInputs.BEGIN_BLOCK);
        }
    }
    public void EVENT_UpBlocking()
    {
        stateMachine.SendInput(PlayerInputs.END_BLOCK);
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
        stateMachine.SendInput(PlayerInputs.PARRY);
    }

    #endregion

    #region Roll
    void OnBeginRoll()
    {
        //Activar trail o feedback x del roll
    }

    void OnEndRoll()
    {
        //desactivar trail o feedback x del roll
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

    void SlowMO()
    {
        Main.instance.GetTimeManager().DoSlowMotion(timeScale, slowDuration);
        customCam.DoFastZoom(speedAnim);
    }

    #region Take Damage
    public override Attack_Result TakeDamage(int dmg, Vector3 attackDir, Damagetype dmgtype)
    {
        if (InDash())
            return Attack_Result.inmune;

        if (charBlock.IsParry(rot.position, attackDir, rot.forward))
        {
            Debug.LogWarning("PARRY perfect");
            PerfectParry();
            Main.instance.GetTimeManager().DoSlowMotion(timeScale, slowDuration);
            customCam.DoFastZoom(10);
            return Attack_Result.parried;
        }
        else if (charBlock.IsBlock(rot.position, attackDir, rot.forward))
        {
            blockParticle.Play();
            charanim.BlockSomething();
            charBlock.SetBlockCharges(-1);
            lifesystem.Hit(dmg / 2);
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
    public int screamsToSkill = 50;

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


    #region Debuggin
    [SerializeField] UnityEngine.UI.Text txt_debug = null;
    void DebugState(string state) { if (txt_debug != null) txt_debug.text = state; }
    void StartDebug() { if (txt_debug != null) txt_debug.enabled = false; DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Character State Machine Debug", false, ToogleDebug); }
    string ToogleDebug(bool active) { if (txt_debug != null) txt_debug.enabled = active; return active ? "debug activado" : "debug desactivado"; }
    #endregion

}