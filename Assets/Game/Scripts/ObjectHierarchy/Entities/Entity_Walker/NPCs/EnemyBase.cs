using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : NPCBase
{
    public virtual void Awake()
    {
        side_Type = side_type.enemy;
    }
}
