using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
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
    #region SCRIPT TEMPORAL, BORRAR
    Action<float> changeCDDash; public void ChangeDashCD(float _cd) => changeCDDash.Invoke(_cd);
    #endregion

[Header("Character Head")]

    CharacterBlock charBlock;
    Action OnBlock;
    Action UpBlock;
    Action Parry;
    Action OnAttackBegin;
    Action OnAttackEnd;
    
    CharacterAttack charAttack;
    CharacterBlock charBlock;
    #region SCRIPT TEMPORAL, BORRAR
    Action<float> changeCDDash; public void ChangeDashCD(float _cd) => changeCDDash.Invoke(_cd);
    #endregion

[Header("Dash Options")]
    [SerializeField] bool directionalDash;
    
    [SerializeField] float dashTiming;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDeceleration;
    [SerializeField] float dashCD;
[SerializeField] float _timerOfParry;
    [SerializeField] float attackRange;
    [SerializeField] float timeToHeavyAttack;

[Header("Movement Options")]
    [SerializeField] float speed;

    [SerializeField] Transform rot;
    CharacterMovement move;

    [Header("Parry & Block Options")]
    [SerializeField] float _timerOfParry;
    
    [SerializeField] ParticleSystem parryParticle;
    [SerializeField] ParticleSystem hitParticle;

    [Header("Life Options")]
    [SerializeField] LifeSystem lifesystem;

    public GameObject feedbackParry;
    public GameObject feedbackBlock;

    

    [SerializeField]
    Text txt;

    CharacterAnimator charanim;
    [SerializeField] Animator anim_base;

    

    [SerializeField] AnimEvent charAnimEvent;

    private void Awake()
    {
        charanim = new CharacterAnimator(anim_base);

        move = new CharacterMovement(GetComponent<Rigidbody>(), rot, IsDirectionalDash, charanim)
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

        charBlock = new CharacterBlock(_timerOfParry, OnBeginParry, OnEndParry, charanim);
        OnBlock += charBlock.OnBlockDown;
        UpBlock += charBlock.OnBlockUp;
        Parry += charBlock.Parry;
        ChildrensUpdates += charBlock.OnUpdate;
        
        charAttack = new CharacterAttack(attackRange, timeToHeavyAttack, charanim, rot);
        OnAttackBegin += charAttack.OnattackBegin;
        OnAttackEnd += charAttack.OnAttackEnd;


        charAnimEvent.Add_Callback("Attack", EVENT_Attack );
        charAnimEvent.Add_Callback("RompeCoco", RompeCoco);
        charAnimEvent.Add_Callback("BeginBlock", charBlock.OnBlockSucesfull);
        charAnimEvent.Add_Callback("BeginBlock", BlockFeedback);


        #region SCRIPT TEMPORAL, BORRAR
        changeCDDash += move.ChangeDashCD;
        #endregion
    }

    void RompeCoco() => lifesystem.Hit(10);

    private void Update()
    {
        ChildrensUpdates();

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            RollDash();
    }
    

    public void BeginAttack()
    {
        OnAttackBegin();
    }

    public void EVENT_Attack()
    {
        charAttack.Attack(1, 2);
    }

    #region Block & Parry

    public void EVENT_OnBlocking()
    {
        if (!charBlock.onParry && !InDash())
        {
            move.SetSpeed(speed / 2);
            OnBlock();
        }
    }
    public void EVENT_UpBlocking()
    {
        feedbackBlock.SetActive(false);
        move.SetSpeed(speed);
        UpBlock();
    }

    public void BlockFeedback()
    {
        feedbackBlock.SetActive(true);
    }
    public void EVENT_Parry()
    {
        if (!charBlock.onBlock && !InDash())
        {
            charanim.Parry();
            Parry();
        }
    }
    public void PerfectParry()
    {
        parryParticle.Play();
    }
    void OnBeginParry() => feedbackParry.SetActive(true);
    void OnEndParry() => feedbackParry.SetActive(false);

    #endregion

#region Attack
     public void EVENT_OnAttackBegin() { OnAttackBegin(); }
    public void EVENT_OnAttackEnd() { }
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

    #region Roll 
    bool IsDirectionalDash()
    {
        return directionalDash;
    }

    public void ChangeDashForm()
    {
        directionalDash = !directionalDash;
        txt.text = "Directional Dash = " + directionalDash.ToString();
    }
    public void RollDash()
    {
        if (!InDash())
        {
            UpBlock();
            Dash();
        }
    }

    #endregion

    #region Take Damage
    public override Attack_Result TakeDamage(int dmg)
    {
        if (InDash())
            return Attack_Result.inmune;

        if (charBlock.onParry)
        {
            PerfectParry();
            return Attack_Result.parried;
        }
        else if (charBlock.onBlock)
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

    #region Fuera de uso
    protected override void OnUpdateEntity() { }
    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    #endregion 
}
