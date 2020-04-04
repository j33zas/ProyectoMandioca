using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWeapon_Component : MonoBehaviour
{
    private CharacterHead _characterHead;
    private float originalAttackRange;
    private float currentAttackrange;

    private void Awake()
    {
        _characterHead = GetComponent<CharacterHead>();

        originalAttackRange = _characterHead.ChangeRangeAttack(-1);
        currentAttackrange = originalAttackRange;
        
        Debug.Log(originalAttackRange + "ataque original");
    }

    public void ChangeWeaponAttackRange(float percent)
    {
        float newRangeValue = currentAttackrange * percent;
        Debug.Log(newRangeValue);
        currentAttackrange = _characterHead.ChangeRangeAttack(newRangeValue);
        
    }

    public void ReturnToOriginalRangeAttack()
    {
        _characterHead.ChangeRangeAttack(originalAttackRange);
    }
}
