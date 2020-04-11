using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatDirector 
{
    Transform CurrentTargetPos();

    void SetTargetPos(Transform pos);

    Vector3 CurrentPos();

    void ToAttack();
}
