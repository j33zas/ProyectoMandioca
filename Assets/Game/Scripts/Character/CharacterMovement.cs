using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMovement
{

    Rigidbody _rb;
    Transform rotTransform;
    float speed;

    float rotX;
    float rotY;

    bool inDash;

    float timerDash;
    float maxTimerDash;
    float dashSpeed;
    float dashCd;
    float cdTimer;
    bool dashCdOk;

    public CharacterMovement(Rigidbody rb, Transform rot, float s, float dTiming, float dSpeed, float dCd)
    {
        _rb = rb;
        speed = s;
        rotTransform = rot;
        maxTimerDash = dTiming;
        dashSpeed = dSpeed;
        dashCd = dCd;
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

    //Joystick Derecho, Rotacion
    public void RightHorizontal(float axis)
    {
        rotX = axis;
        Rotation(rotTransform.forward.z, axis);
    }

    public void RightVerical(float axis)
    {
        rotY = axis;
        Rotation(axis, rotTransform.forward.x);
    }

    public void OnUpdate()
    {
        if (inDash)
        {
            timerDash += Time.deltaTime;

            _rb.velocity = rotTransform.forward * dashSpeed;

            if (timerDash >= maxTimerDash)
            {
                inDash = false;
                _rb.velocity = Vector3.zero;
                timerDash = 0;
            }
        }

        if (dashCdOk)
        {
            cdTimer += Time.deltaTime;

            if (cdTimer >= dashCd)
            {
                dashCdOk = false;
                cdTimer = 0;
            }
        }
    }

    void Rotation(float axisX, float axisY)
    {
        Vector3 dir = rotTransform.forward + new Vector3(axisY, 0, axisX);

        if (dir == Vector3.zero)
            rotTransform.forward = new Vector3(axisY, 0, axisX);
        else
            dir = new Vector3(axisY, 0, axisX);

        rotTransform.forward += dir;
    }

    void Move(float axisX, float axisY)
    {

        float velY = _rb.velocity.y;

        _rb.velocity = new Vector3(axisX, velY, axisY);


        if (rotX >= 0.3 || rotX <= -0.3 || rotY >= 0.3 || rotY <= -0.3)
        {
            Rotation(rotY, rotX);
        }
        else
        {
            Rotation(axisY, axisX);
        }
    }

    public void Roll()
    {
        if (dashCdOk)
            return;

        inDash = true;
        dashCdOk = true;
    }

    public bool IsDash()
    {
        return inDash;
    }
}
