using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Follow : MonoBehaviour
{
    public float rotationSpeed;
    Rigidbody rb;
    float speed;
    public float distanceStop = 1;

    Action End;

    Vector3 target;

    bool run;

    public void Configure(Rigidbody _rb, float _speed, Action _End)
    {
        rb = _rb;
        speed = _speed;
        End = _End;
    }

    public void SetTarget(Vector3 _target)
    {
        target = _target;
    }


    public void Execute()
    {
        if (Vector3.Distance(transform.forward, target) > distanceStop)
        {
            float velY = rb.velocity.y;
            rb.velocity = new Vector3(target.x * speed, target.y + velY, target.z * speed);
            transform.forward = Vector3.Lerp(transform.forward, target, rotationSpeed * Time.deltaTime);
        }
        else
        {
            End.Invoke();
        }
    }

    public void NewExecute(Transform target)
    {
        if (Vector3.Distance(rb.transform.position, target.position) > distanceStop)
        {
            Vector3 dir = target.position - rb.transform.position;
            dir.Normalize();
            rb.velocity = new Vector3(dir.x * speed, 0, dir.z * speed);
        }
        else
        {
            End.Invoke();
        }
    }
}
