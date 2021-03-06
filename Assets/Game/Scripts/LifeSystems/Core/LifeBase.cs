﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifeBase : StatBase
{
    FrontendStatBase uilife;

    public LifeBase(int maxHealth, int initial_Life = -1) : base(maxHealth, initial_Life) { }
    public LifeBase(int maxHealth, FrontendStatBase _uilife, int initial_Life = -1) : base(maxHealth, initial_Life)
    {
        uilife = _uilife;
        uilife.OnValueChange(maxHealth, maxHealth);
    }

    public event Action loselife;
    public event Action gainlife;
    public event Action death;
    public event Action cannotAddMore = delegate { };
    public event Action cannotRemoveMore = delegate { };
    public event Action<int, int> lifechange = delegate { };

    public void AddEventListener_LoseLife(Action listener) { loselife += listener; }
    public void AddEventListener_GainLife(Action listener) { gainlife += listener; }
    public void AddEventListener_Death(Action listener) { death += listener; }
    public void AddEventListener_CannotAddMore(Action listener) { cannotAddMore += listener; }
    public void AddEventListener_CannotRemoveMore(Action listener) { cannotRemoveMore += listener; }
    public void AddEventListener_OnLifeChange(Action<int, int> listener) { lifechange += listener; }


    public override void OnAdd() { gainlife.Invoke(); }
    public override void OnRemove() { loselife.Invoke(); }
    public override void OnLoseAll() { death.Invoke(); }
    public override void CanNotAddMore() { cannotAddMore.Invoke(); }
    public override void CanNotRemoveMore() { cannotRemoveMore.Invoke(); }

    public override void OnValueChange(int value, int max)
    {
        lifechange.Invoke(value, max);
        if (uilife != null) uilife.OnValueChange(value, max);
    }
}
