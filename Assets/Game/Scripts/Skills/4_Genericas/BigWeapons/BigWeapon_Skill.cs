using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWeapon_Skill : SkillBase
{
    [Header("_-_Skill Settings_-_")]
    [SerializeField] private int hitsToActivate;
    [SerializeField] private float duration;
    [Range(0,10)]
    [SerializeField] private float percentRangeModifier;

    [Header("Feedback settings")]
    [SerializeField] private Transform aura;
    [SerializeField] Transform ghostSword;
    [SerializeField] Transform atenaImage;
    
    
    private float _count;
    private int cantDeGolpesExitosos;
    private CharacterAttack _characterAttack;
    private CharacterHead charHead;
    private float originalrange;
    private bool isActive = false;

    private float currentAttackRange;
    protected override void OnBeginSkill()
    {
        charHead = Main.instance.GetChar();
        _characterAttack = Main.instance.GetChar().GetCharacterAttack();

        _characterAttack.currentWeapon.AttackResult += OnSuccesAttack;

        originalrange = _characterAttack.currentWeapon.ModifyAttackrange();

    }
    private void OnSuccesAttack(Attack_Result result) // recibe el resultado del ataque y suma uno al contador. Si llega al ncesario, activa el skill
    {
        if (isActive) return;
        
        if (result == Attack_Result.sucessful) cantDeGolpesExitosos++;

        if (cantDeGolpesExitosos == hitsToActivate)
        {
            cantDeGolpesExitosos = 0;
            isActive = true;
            _characterAttack.currentWeapon.ModifyAttackrange(CalculateRangeAttackModifier(percentRangeModifier));
            aura.position = charHead.transform.position + Vector3.up * .25f;
            aura.gameObject.SetActive(true);

            charHead.GetCharacterAttack().OnAttack += SetGhostSword;
        }
    }

    private void SetGhostSword()
    {
        atenaImage.transform.position = charHead.transform.position + Vector3.up * 3;
        atenaImage.GetComponent<ParticleSystem>().Play();
        ghostSword.localPosition = charHead.transform.position + charHead.GetCharMove().GetLookDirection() * _characterAttack.currentWeapon.GetWpnRange() + Vector3.up;
        ghostSword.rotation = Quaternion.LookRotation(charHead.GetCharMove().GetLookDirection(), Vector3.up);
        ghostSword.gameObject.SetActive(true);
    }

    protected override void OnEndSkill()
    {
        _characterAttack.currentWeapon.AttackResult -= OnSuccesAttack;
        _characterAttack.currentWeapon.ModifyAttackrange(originalrange);
    }

    protected override void OnUpdateSkill()
    {
        atenaImage.transform.position = charHead.transform.position + Vector3.up * 3;
        aura.position = charHead.transform.position + Vector3.up * .25f;
        
        if (isActive)
        {
            _count += Time.deltaTime;
            if (_count >= duration)
            {
                _count = 0;
                isActive = false;
                charHead.GetCharacterAttack().OnAttack -= SetGhostSword;
            }
        }
        else
        {
            aura.gameObject.SetActive(false);
            _characterAttack.currentWeapon.ModifyAttackrange(originalrange);
            
        }
    }
    
    public float CalculateRangeAttackModifier(float percent)
    {
        float newRangeValue = originalrange * percent;
        return newRangeValue;

    }
}
