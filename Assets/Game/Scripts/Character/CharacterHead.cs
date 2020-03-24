using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


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

    #region SCRIPT TEMPORAL, BORRAR
    Action<float> changeCDDash; public void ChangeDashCD(float _cd) => changeCDDash.Invoke(_cd);
    #endregion

    [SerializeField]
    bool directionalDash;

    [SerializeField]
    float speed;
    [SerializeField]
    float dashTiming;
    [SerializeField]
    float dashSpeed;
    [SerializeField]
    float dashCD;

    [SerializeField]
    Transform rot;

    Func<bool> InDash;
    //el head va a recibir los inputs
    //primero pasa por aca y no directamente al movement porque tal vez necesitemos extraer la llamada
    //para visualizarlo con algun feedback visual

    [SerializeField]
    Text txt;


    private void Awake()
    {
        var move = new CharacterMovement(GetComponent<Rigidbody>(), rot, speed, dashTiming,dashSpeed, dashCD, IsDirectionalDash);

        MovementHorizontal += move.LeftHorizontal;
        MovementVertical += move.LeftVerical;
        RotateHorizontal += move.RightHorizontal;
        RotateVertical += move.RightVerical;
        Dash += move.Roll;
        InDash += move.IsDash;
        ChildrensUpdates += move.OnUpdate;

        var charblock = new CharacterBlock();
        OnBlock += charblock.OnBlockDown;
        UpBlock += charblock.OnBlockUp;
        Parry += charblock.Parry;



        #region SCRIPT TEMPORAL, BORRAR
        changeCDDash += move.ChangeDashCD;
        #endregion
    }

    bool IsDirectionalDash()
    {
        return directionalDash;
    }

    public void ChangeDashForm()
    {
        directionalDash = !directionalDash;
        txt.text = "Directional Dash = " + directionalDash.ToString();
    }
    // esto es para testing //LUEGO QUE CUMPLA SU FUNCION... BORRAR
    

    private void Update()
    {
        ChildrensUpdates();

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            RollDash();
    }

    public void EVENT_OnBlocking()
    {
        OnBlock();
    }
    public void EVENT_UpBlocking()
    {
        UpBlock();
    }
    public void EVENT_Parry()
    {
        Parry();
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
            Dash();

    }

    protected override void OnUpdateEntity() { }
    public override void TakeDamage(int dmg) { }
    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
}
