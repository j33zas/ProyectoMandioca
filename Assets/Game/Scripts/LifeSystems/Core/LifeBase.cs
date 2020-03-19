using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifeBase : StatBase
{
    FrontendStatBase uilife;

    public LifeBase(int maxHealth, FrontendStatBase _uilife, int initial_Life = -1) : base(maxHealth, initial_Life)
    {
        uilife = _uilife;
        uilife.OnValueChange(maxHealth, maxHealth);
    }

    public event Action loselife;
    public event Action gainlife;
    public event Action death;

    public void AddEventListener_LoseLife(Action listener) { loselife += listener; }
    public void AddEventListener_GainLife(Action listener) { gainlife += listener; }
    public void AddEventListener_Death(Action listener) { death += listener; }

    public override void OnAdd()
    {
        Debug.Log("OnAdd");
    }

    public override void OnRemove()
    {
        Debug.Log("OnRemove");
    }

    public override void OnLoseAll()
    {
        Debug.Log("OnLoseAll");
    }

    public override void CanNotAddMore()
    {
        Debug.Log("CannotAddMOre");
    }

    public override void CanNotRemoveMore()
    {
        Debug.Log("CanNotRemoveMove");
    }

    public override void OnValueChange(int value, int max)
    {
        if(uilife != null) uilife.OnValueChange(value, max);
    }
}
