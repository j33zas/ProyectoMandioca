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

        Move(axis * speed, velZ);

    }

    public void LeftVerical(float axis)
    {
        float velX = _rb.velocity.x;

        Move(velX, axis * speed);
    }

    void Move(float axisX, float axisY)
    {
        float velY = _rb.velocity.y;

        _rb.velocity = new Vector3(axisX, velY, axisY);

        Vector3 dir = new Vector3(axisX, 0, axisY);
        rotTransform.forward += dir.normalized;

        if (rotTransform.forward.x == 0)
        {
            rotTransform.forward = new Vector3(rotTransform.transform.forward.x, rotTransform.transform.forward.x, -1);
        }
    }

    //Joystick Derecho, Rotacion
    public void RightHorizontal(float axis)
    {
        Rotation(rotTransform.forward.z, axis);
    }

    public void RightVerical(float axis)
    {
        Rotation(axis, rotTransform.forward.x);
    }

    void Rotation(float axisX, float axisY)
    {
        rotTransform.forward += new Vector3(axisY, 0, axisX);

        if (rotTransform.forward.z == 0)
        {
            rotTransform.forward += new Vector3(rotTransform.transform.forward.x, rotTransform.transform.forward.x, -1);
        }
    }
}
