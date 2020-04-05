using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWeapon_Skill : SkillBase
{
    private BigWeapon_Component _bigWeaponComponent;

    [Range(-10,10)]
    [SerializeField] private float percentRangeModifier;
    protected override void OnBeginSkill()
    {
        if(_bigWeaponComponent == null)
            _bigWeaponComponent = FindObjectOfType<CharacterHead>().GetComponent<BigWeapon_Component>();
        
        
        _bigWeaponComponent.ChangeWeaponAttackRange(percentRangeModifier);
    }

    protected override void OnEndSkill()
    {
        _bigWeaponComponent.ReturnToOriginalRangeAttack();
    }

    protected override void OnUpdateSkill()
    {
        
    }
}
