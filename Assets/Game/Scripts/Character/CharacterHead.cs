using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHead : MonoBehaviour
{

    //el head va a recibir los inputs
    //primero pasa por aca y no directamente al movement porque tal vez necesitemos extraer la llamada
    //para visualizarlo con algun feedback visual

    //Joystick Izquierdo, Movimiento
    public void LeftHorizontal(float axis)
    {
        //envio datos
        //feedback
        //etc
    }
    public void LeftVerical(float axis)
    { 
        //envio datos
        //feedback
        //etc
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
