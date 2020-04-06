using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesWander : States
{
    bool hasTarget;

    public StatesWander(StatesMachine SM) : base(SM)
    {

    }

    public override void Start()
    {
        base.Start();
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log("Minion wandering");
    }

    public override void Sleep()
    {
        base.Sleep();
    }
}
