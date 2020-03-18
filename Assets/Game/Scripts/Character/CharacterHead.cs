using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CharacterHead : MonoBehaviour
{
    Action<float> MovementHorizontal;
    Action<float> MovementVertical;
    Action<float> RotateHorizontal;
    Action<float> RotateVertical;


    [SerializeField]
    float speed;
    [SerializeField]
    Transform rot;
    //el head va a recibir los inputs
    //primero pasa por aca y no directamente al movement porque tal vez necesitemos extraer la llamada
    //para visualizarlo con algun feedback visual

    private void Awake()
    {
        var move = new CharacterMovement(GetComponent<Rigidbody>(), rot, speed);

        MovementHorizontal += move.LeftHorizontal;
        MovementVertical += move.LeftVerical;
        RotateHorizontal += move.RightHorizontal;
        RotateVertical += move.RightVerical;
    }

    //Joystick Izquierdo, Movimiento
    public void LeftHorizontal(float axis)
    {
        MovementHorizontal(axis);
    }

    public void LeftVerical(float axis)
    {
        MovementVertical(axis);
    }

    //Joystick Derecho, Rotacion
    public void RightHorizontal(float axis)
    {
        RotateHorizontal(axis);
    }
    public void RightVerical(float axis)
    {
        RotateVertical(axis);
    }
}
