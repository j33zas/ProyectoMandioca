using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMovement
{

    Rigidbody _rb;
    Transform rotTransform;
    float speed;

    public CharacterMovement(Rigidbody rb, Transform rot, float s)
    {
        _rb = rb;
        speed = s;
        rotTransform = rot;
    }

    //Joystick Izquierdo, Movimiento
    public void LeftHorizontal(float axis)
    {
        float velZ = _rb.velocity.z;

        Move(axis, velZ);

    }

    public void LeftVerical(float axis)
    {
        float velX = _rb.velocity.x;

        Move(velX, axis);
    }

    void Move(float axisX, float axisY)
    {
        float velY = _rb.velocity.y;

        _rb.velocity = new Vector3(axisX * speed, velY, axisY * speed);

        rotTransform.forward += new Vector3(axisX, 0, axisY);
    }

    //Joystick Derecho, Rotacion
    public void RightHorizontal(float axis)
    {
        Rotation(rotTransform.forward.x, axis);
    }

    public void RightVerical(float axis)
    {
        Rotation(axis, rotTransform.forward.z);
    }

    void Rotation(float axisX, float axisY)
    {
        //rotTransform.forward += new Vector3(axisX, 0, axisY);
        //Debug.Log(axisX + "             " + axisY);
    }
}
