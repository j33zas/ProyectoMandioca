using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMovement
{

    Rigidbody _rb;
    float speed;

    public CharacterMovement(Rigidbody rb, float s)
    {
        _rb = rb;
        speed = s;
    }

    //Joystick Izquierdo, Movimiento
    public void LeftHorizontal(float axis)
    {
        float velY = _rb.velocity.y;
        float velZ = _rb.velocity.z;

        _rb.velocity = new Vector3(axis * speed, velY, velZ);

        Debug.Log(axis);
    }

    public void LeftVerical(float axis)
    {
        float velY = _rb.velocity.y;
        float velX = _rb.velocity.x;

        _rb.velocity = new Vector3(velX, velY, axis * speed);
    }

    //Joystick Derecho, Rotacion
    public void RightHorizontal(float axis) { }
    public void RightVerical(float axis) { }
}
