using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : NPCBase
{
    public bool target;
    public GameObject _targetFeedback;
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
}
