using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CharacterHead : MonoBehaviour
{
    Action<float> MovementHorizontal;
    Action<float> MovementVertical;
    [SerializeField]
    float speed;
    //el head va a recibir los inputs
    //primero pasa por aca y no directamente al movement porque tal vez necesitemos extraer la llamada
    //para visualizarlo con algun feedback visual

    private void Awake()
    {
        var move = new CharacterMovement(GetComponent<Rigidbody>(), speed);

        MovementHorizontal += move.LeftHorizontal;
        MovementVertical += move.LeftVerical;
    }

    //Joystick Izquierdo, Movimiento
    public void LeftHorizontal(float axis)
    {
        MovementHorizontal(axis);

        Debug.Log(axis);
    }

    public void LeftVerical(float axis)
    {
        MovementVertical(axis);
    }

    //Joystick Derecho, Rotacion
    public void RightHorizontal(float axis)
    { 
        //envio datos
        //feedback
        //etc
    }
    public void RightVerical(float axis)
    { 
        //envio datos
        //feedback
        //etc
    }
}
