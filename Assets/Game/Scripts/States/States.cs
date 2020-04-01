using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class States
{
    protected StatesMachine statemachine;
    public States(StatesMachine SM)
    {
        statemachine = SM;
    }

    public virtual void Start()
    {

    }
    public virtual void Sleep()
    {

    }
    public virtual void Execute()
    {

    }
    public virtual void lateExecute()
    {

    }
}
