using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyBase : NPCBase, ICombatDirector
{
    public bool target;
    public bool attacking;
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

    [SerializeField] protected float combatDistance;
    protected bool combat;

    public void Mortal()
    {
        Invinsible = false;
    }

    #region Combat Director Functions
    protected bool withPos;


    protected EntityBase entityTarget;

    public Transform _target;

    [SerializeField, Range(0.5f, 15)] float distancePos = 1.5f;

    public Transform CurrentTargetPos()
    {
        return _target;
    }

    public void SetTargetPosDir(Transform pos)
    {
        _target = pos;
        _target.localPosition *= distancePos;
    }

    public Vector3 CurrentPos()
    {
        return transform.position;
    }

    public void SetTarget(EntityBase entity)
    {
        entityTarget = entity;
    }

    public bool IsInPos() { return withPos; }

    public EntityBase CurrentTarget() { return entityTarget; }

    public Transform CurrentTargetPosDir()
    {
        _target.localPosition /= distancePos;
        return _target;
    }

    public void SetBool(bool isPos)
    {
        withPos = isPos;
    }

    public abstract void ToAttack();

    public abstract void IAInitialize(CombatDirector _director);

    public abstract float ChangeSpeed(float newSpeed);
    #endregion

    protected bool IsAttack() { return attacking; }

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
