using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWeapon_Skill : SkillBase
{
    private CharacterAttack _characterAttack;

    [Range(-10,10)]
    [SerializeField] private float percentRangeModifier;

    [SerializeField] private Vector3 augmentedSize;
    private Vector3 originalSize;

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
        originalSize = Main.instance.GetChar().currentWeapon.transform.localScale;
        Main.instance.GetChar().currentWeapon.transform.localScale = new Vector3(augmentedSize.x, augmentedSize.y, augmentedSize.z);
        _characterAttack.currentWeapon.ModifyAttackrange(CalculateRangeAttackModifier(percentRangeModifier));
        
    }

    protected override void OnEndSkill()
    {
        _characterAttack.currentWeapon.ModifyAttackrange();
        Main.instance.GetChar().currentWeapon.transform.localScale = originalSize;
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
