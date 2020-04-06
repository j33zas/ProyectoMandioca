using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWeapon_Skill : SkillBase
{
    private CharacterAttack _characterAttack;

    [Range(-10,10)]
    [SerializeField] private float percentRangeModifier;

    private float currentAttackRange;
    protected override void OnBeginSkill()
    {
        if (_characterAttack == null)
        {
            if (Main.instance == null)
            {
                _characterAttack = FindObjectOfType<CharacterHead>().GetCharacterAttack();
            }
            else
            {
                _characterAttack = Main.instance.GetChar().GetCharacterAttack();     
            }
            
           
        }
            

        currentAttackRange = _characterAttack.currentWeapon.ModifyAttackrange();
        _characterAttack.currentWeapon.ModifyAttackrange(CalculateRangeAttackModifier(percentRangeModifier));
        
    }

    protected override void OnEndSkill()
    {
        _characterAttack.currentWeapon.ModifyAttackrange();
    }

    protected override void OnUpdateSkill()
    {
        
    }
    
    public float CalculateRangeAttackModifier(float percent)
    {
        float newRangeValue = currentAttackRange * percent;
        return newRangeValue;

    }

    private void OnDrawGizmos()
    {
        
    }
}
