using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DevelopTools;

public class CharacterHead : CharacterControllable
{
    Action<float> MovementHorizontal;
    Action<float> MovementVertical;
    Action<float> RotateHorizontal;
    Action<float> RotateVertical;
    Action Dash;
    Action ChildrensUpdates;
    Func<bool> InDash;

    CharacterBlock charBlock;
    Action OnBlock;
    Action UpBlock;
    Action Parry;


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

    [Header("Life Options")]
    [SerializeField] LifeSystem lifesystem;

    [Header("Feedbacks")]
    public GameObject feedbackParry;
    public GameObject feedbackBlock;
    [SerializeField] ParticleSystem feedbackCW;


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
    float dmg = 5;
    Action OnAttackBegin;
    Action OnAttackEnd;
    CharacterAttack charAttack;

    CustomCamera customCam;

    public Action Attack;
    public Action ChangeWeaponPassives = delegate { };

    //Modelo del arma para feedback placeholder
    public GameObject currentWeapon;

    [Header("Interactable")]
    public InteractSensor sensor;

    bool canBlockCalculate;

    private void Awake()
    {
        charanim = new CharacterAnimator(anim_base);
        customCam = FindObjectOfType<CustomCamera>();

        move = new CharacterMovement(GetComponent<Rigidbody>(), rot, charanim)
            .SetSpeed(speed)
            .SetTimerDash(dashTiming).SetDashSpeed(dashSpeed)
            .SetDashCD(dashCD)
            .SetRollDeceleration(dashDeceleration);

        MovementHorizontal += move.LeftHorizontal;
        MovementVertical += move.LeftVerical;
        RotateHorizontal += move.RightHorizontal;
        RotateVertical += move.RightVerical;
        Dash += move.Roll;
        InDash += move.IsDash;
        ChildrensUpdates += move.OnUpdate;

        charBlock = new CharacterBlock(_timerOfParry, blockAngle, OnEndParry, charanim, feedbackBlock);
        OnBlock += charBlock.OnBlockDown;
        UpBlock += charBlock.OnBlockUp;
        Parry += charBlock.Parry;
        Parry += OnBeginParry;
        ChildrensUpdates += charBlock.OnUpdate;

        charAttack = new CharacterAttack(attackRange, attackAngle, timeToHeavyAttack, charanim, rot, ReleaseInNormal, ReleaseInHeavy, feedbackHeavy, rangeOfPetrified, dmg);
        OnAttackBegin += charAttack.OnattackBegin;
        OnAttackEnd += charAttack.OnAttackEnd;
        charAttack.FirstAttackReady(true);

        charAnimEvent.Add_Callback("CheckAttackType", CheckAttackType);
        charAnimEvent.Add_Callback("DealAttack", DealAttack);
        charAnimEvent.Add_Callback("RompeCoco", RompeCoco);
        charAnimEvent.Add_Callback("BeginBlock", charBlock.OnBlockSuccessful);

        move.SetCallbacks(OnBeginRoll, OnEndRoll);

        Attack += charAttack.Attack;
    }

    private void Update()
    {
        ChildrensUpdates();
        charAttack.Refresh();
    }

    #region Attack
    /////////////////////////////////////////////////////////////////

    public void EVENT_OnAttackBegin() { OnAttackBegin(); }
    public void EVENT_OnAttackEnd() { OnAttackEnd(); }

    //tengo la espada arriba
    public void CheckAttackType()
    {
        charAttack.BeginCheckAttackType();
    }

    public void DealAttack()
    {
        Attack.Invoke();
        //charAttack.Attack();
    }

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
    public void ChangeRangeAttack()
    {
        charAttack.currentWeapon.ModifyAttackrange();
    }

    public CharacterAttack GetCharacterAttack()
    {
        return charAttack;
    }


    private void OnDrawGizmos()
    {
        if(charAttack == null)
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

    public void EVENT_OnBlocking()
    {
        if (!charBlock.onParry && !InDash() && !charAttack.inAttack)
        {
            Debug.Log("Entrar a animacion");
            move.SetSpeed(speed / 2);
            OnBlock();
        }
    }
    public void EVENT_UpBlocking()
    {
        if (canBlockCalculate)
            canBlockCalculate = false;
        else
        {
            if (!charBlock.onParry && !InDash() && !charAttack.inAttack)
            {
                move.SetSpeed(speed);
                UpBlock();
            }
        }
    }
    public void OnRealBlock_ON()
    {

    }
    public void OnRealBlock_OFF()
    {

    }

    public void EVENT_Parry()
    {
        if (!charBlock.onBlock && !InDash() && !charAttack.inAttack)
        {
            charanim.Parry();
            Parry();
        }
    }
    public void AddParry(Action listener)
    {
        Parry += listener;
    }
    public void RemoveParry(Action listener)
    {
        Parry -= listener;
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
        charBlock.flagIsStop = false;
        charBlock.onBlock = false;
        charanim.Block(false);
        // canBlockCalculate = false;
    }
    void OnEndRoll()
    {
        charanim.Block(false);
        charBlock.flagIsStop = false;
        charBlock.onBlock = false;
        if (Input.GetButton("Block"))
            canBlockCalculate = true;
    }
    public void RollDash()
    {
        if (!InDash())
        {
            if (charBlock.onBlock)
            {
                EVENT_UpBlocking();
            }
            Dash();
        }
    }

    public void AddListenerToDash(Action listener) => Dash += listener;
    public void RemoveListenerToDash(Action listener) => Dash -= listener;
    public void ChangeDashForTeleport()
    {
        Dash -= move.Roll;
        Dash += move.Teleport;
    }
    public void ChangeTeleportForDash()
    {
        Dash -= move.Teleport;
        Dash += move.Roll;
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
    public void LeftHorizontal(float axis)
    {
        if (!InDash())
            MovementHorizontal(axis);
    }

    public void LeftVerical(float axis)
    {
        if (!InDash())
            MovementVertical(axis);
    }

    //Joystick Derecho, Rotacion
    public void RightHorizontal(float axis)
    {
        if (!InDash())
            RotateHorizontal(axis);
    }
    public void RightVerical(float axis)
    {
        if (!InDash())
            RotateVertical(axis);
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
    public void EVENT_OnInteractDown()
    {
        sensor.OnInteractDown();
    }
    public void EVENT_OnInteractUp()
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

    #endregion

    #region Fuera de uso
    protected override void OnUpdateEntity() { }
    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    #endregion 
}