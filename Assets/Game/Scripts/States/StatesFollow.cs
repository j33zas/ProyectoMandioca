﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesFollow : States
{
    protected float rotationSpeed;
    protected float distanceStop;
    protected Transform _myTransform;
    protected Rigidbody _rb;
    protected Transform _target;
    protected float speed;
    protected Animator _anim;
    bool run;
     public StatesFollow(StatesMachine sm,Transform trans,Rigidbody rb,Transform target,Animator anim,float rotSpeed,float distance) : base(sm)
     {
        _myTransform = trans;
        _rb = rb;
        _target = target;
        _anim = anim;
        rotationSpeed = rotSpeed;
        distanceStop = distance;
        speed = 4;
     }

    public override void Start()
    {
        base.Start();

    }

    public override void Execute()
    {
        base.Execute();
        if (Vector3.Distance(_myTransform.position, _target.position) > distanceStop)
        {
           
            Vector3 _dir = (_target.position - _myTransform.position).normalized;
            Vector3 velocity= new Vector3(_dir.x, 0, _dir.z);
            _rb.velocity = velocity * speed;
           
            _myTransform.forward = Vector3.Lerp(_myTransform.forward, _dir, rotationSpeed * Time.deltaTime);
        }
        else
        {
            statemachine.ChangeState<StatesAttack>();
        }
    }

    public override void Sleep()
    {
        base.Sleep();
    }
}
