using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyBase : NPCBase, ICombatDirector
{
    public bool target;
    protected bool attacking;
    public GameObject _targetFeedback;
    public Action OnParried;
    public bool minionTarget;
    public bool Invinsible;
    public virtual void Awake()
    {
        side_Type = side_type.enemy;
    }
    public virtual void IsTarget()
    {
        target = true;
        _targetFeedback.SetActive(true);
    }
    public virtual void IsNormal()
    {
        target = false;
        _targetFeedback.SetActive(false);
    }

    public void Mortal()
    {
        Invinsible = false;
    }

    protected Transform _target;

    public Transform CurrentTargetPos()
    {
        return _target;
    }

    public void SetTargetPos(Transform pos)
    {
        _target = pos;
    }

    public Vector3 CurrentPos()
    {
        return transform.position;
    }

    protected bool IsAttack() { return attacking; }

    public abstract void ToAttack();

    public abstract void IAInitialize(CombatDirector _director);

    public abstract float ChangeSpeed(float newSpeed);

    public virtual void GetFocusedOnParry()
    {
        foreach (var item in Main.instance.GetEnemies())
        {
            if (item != this)
                item.minionTarget = false;
            else
                minionTarget = true;
        }
    }
}
