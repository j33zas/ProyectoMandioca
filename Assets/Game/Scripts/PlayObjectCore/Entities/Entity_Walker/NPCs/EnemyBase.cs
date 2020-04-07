using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : NPCBase
{
    public bool target;
    public GameObject _targetFeedback;
    public virtual void Awake()
    {
        side_Type = side_type.enemy;
    }
    public virtual void IsTarget()
    {
        target = true;
        _targetFeedback.SetActive(true);
    }
}
