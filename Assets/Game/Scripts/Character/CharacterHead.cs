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

    Action OnBlock;
    Action UpBlock;
    Action Parry;

    CharacterBlock charBlock;
    #region SCRIPT TEMPORAL, BORRAR
    Action<float> changeCDDash; public void ChangeDashCD(float _cd) => changeCDDash.Invoke(_cd);
    #endregion

    [Header("Character Head")]
    [SerializeField] bool directionalDash;

    [SerializeField] float speed;
    [SerializeField] float dashTiming;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDeceleration;
    [SerializeField] float dashCD;
    [SerializeField] float _timerOfParry;
    [SerializeField] Transform rot;
    [SerializeField] ParticleSystem parryParticle;
    [SerializeField] ParticleSystem hitParticle;

    [SerializeField] LifeSystem lifesystem;

    public GameObject feedbackParry;
    public GameObject feedbackBlock;

    Func<bool> InDash;
    //el head va a recibir los inputs
    //primero pasa por aca y no directamente al movement porque tal vez necesitemos extraer la llamada
    //para visualizarlo con algun feedback visual

    [SerializeField]
    Text txt;

    CharacterAnimator charanim;
    [SerializeField] Animator anim_base;

    CharacterMovement move;

    [SerializeField] CharacterAnimEvent charAnimEvent;


    
    private void Awake()
    {
        //        Animator anim = GetComponent<Animator>();

        charanim = new CharacterAnimator(anim_base);

        move = new CharacterMovement(GetComponent<Rigidbody>(), rot, IsDirectionalDash,charanim).
            SetSpeed(speed).SetTimerDash(dashTiming).SetDashSpeed(dashSpeed).
            SetDashCD(dashCD).SetRollDeceleration(dashDeceleration);

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


        charAnimEvent.Add_Callback("RompeCoco", RompeCoco);
        charAnimEvent.Add_Callback("BeginBlock", charBlock.OnBlockSucesfull);
        charAnimEvent.Add_Callback("BeginBlock", BlockFeedback);


        #region SCRIPT TEMPORAL, BORRAR
        changeCDDash += move.ChangeDashCD;
        #endregion
    }

    void RompeCoco(params object[] obj) => lifesystem.Hit(10);
    

    bool IsDirectionalDash()
    {
        return directionalDash;
    }

    public void ChangeDashForm()
    {
        directionalDash = !directionalDash;
        txt.text = "Directional Dash = " + directionalDash.ToString();
    }

    void OnBeginParry() => feedbackParry.SetActive(true);
    void OnEndParry() => feedbackParry.SetActive(false);



    private void Update()
    {
        ChildrensUpdates();

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            RollDash();
    }

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

    public void BlockFeedback(params object[] objs) 
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

    public void EVENT_OnAttackBegin() { }
    public void EVENT_OnAttackEnd() { }

    //Joystick Izquierdo, Movimiento
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

    public void RollDash()
    {
        if (!InDash())
        {
            UpBlock();
            Dash();
        }
    }

    protected override void OnUpdateEntity() { }
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
            lifesystem.Hit(5);
            return Attack_Result.sucessful;
        }

    }
    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
}
