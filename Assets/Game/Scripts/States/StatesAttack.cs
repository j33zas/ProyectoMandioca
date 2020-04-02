using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesAttack:States
{
    protected float rotationSpeed;
    protected float distanceToFollow;
    protected Transform _myTransform;
    protected Transform _target;
    protected float speed;
    protected Animator _anim;
    public StatesAttack(StatesMachine sm,Animator anim,Transform myTransform,Transform target,float rotSpeed, float distance) :base(sm)
    {
        _myTransform = myTransform;
        _target = target;
        _anim = anim;
        rotationSpeed = rotSpeed;
        distanceToFollow = distance;
    }

    public override void Start()
    {
        base.Start();
        _anim.SetBool("Attack", true);
        
    }

    public override void Execute()
    {
        base.Execute();
        if (Vector3.Distance(_myTransform.position, _target.position) <= distanceToFollow)
        {
            Vector3 _dir = (_target.position - _myTransform.position).normalized;
            _myTransform.forward = Vector3.Lerp(_myTransform.forward, _dir, rotationSpeed * Time.deltaTime);

        }
        else
        {
            statemachine.ChangeState<StatesFollow>();
        }
    }

    public override void Sleep()
    {
        base.Sleep();
        _anim.SetBool("Attack", false);
    }
}
