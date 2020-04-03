using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesPetrified : States
{
    float _timer;
    float _currentTimer;
    public StatesPetrified(StatesMachine sm,float timer) : base(sm)
    {
        _timer = timer;
    }

    public override void Start()
    {
        base.Start();
        _currentTimer = _timer;
    }

    public override void Execute()
    {
        base.Execute();
        _currentTimer -= Time.deltaTime;
        if (_currentTimer <= 0)
        {
            statemachine.ChangeState<StatesFollow>();
        }
    }

    public override void Sleep()
    {
        base.Sleep();
    }
}
