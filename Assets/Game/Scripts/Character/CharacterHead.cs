using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class CharacterHead : MonoBehaviour
{
    Action<float> MovementHorizontal;
    Action<float> MovementVertical;
    Action<float> RotateHorizontal;
    Action<float> RotateVertical;
    Action Dash;
    Action ChildrensUpdates;

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

    private void Update()
    {
        ChildrensUpdates();

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            RollDash();
    }

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
}
