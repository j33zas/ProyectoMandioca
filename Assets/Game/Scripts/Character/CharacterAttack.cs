using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterAttack
{
    Transform forwardPos;
    float range;
    float heavyAttackTime = 1f;
    float buttonPressedTime;
    float angleOfAttack;

    CharacterAnimator anim;

    bool inCheck;

    Action NormalAttack;
    Action HeavyAttack;
    
    bool isAttackReleased;
    bool isAnimationFinished;
    ParticleSystem feedbackHeavy;

    bool oneshot;

    public bool inAttack;
    

    public CharacterAttack(float _range, float _angle, float _heavyAttackTime, CharacterAnimator _anim, Transform _forward, Action _normalAttack, Action _heavyAttack, ParticleSystem ps)
    {
        range = _range;
        angleOfAttack = _angle;
        heavyAttackTime = _heavyAttackTime;
        anim = _anim;
        forwardPos = _forward;

        NormalAttack = _normalAttack;
        HeavyAttack = _heavyAttack;
        feedbackHeavy = ps;
    }

    public void Refresh()
    {
        if (inCheck)
        {
            buttonPressedTime += Time.deltaTime;

            if (buttonPressedTime >= heavyAttackTime)
            {
                if (!oneshot)
                {
                    feedbackHeavy.Play();
                    oneshot = true;
                }
               
            }
        }
    }

    public void OnattackBegin()
    {
        inCheck = true;
        buttonPressedTime = 0f;
        anim.OnAttackBegin();
    }

    void Check()
    {
        inCheck = false;

        if (buttonPressedTime < heavyAttackTime)
        {
            NormalAttack.Invoke();
        }
        else
        {
            HeavyAttack.Invoke();
        }
        feedbackHeavy.Stop();
        oneshot = false;
        buttonPressedTime = 0f;
        isAnimationFinished = false;
        isAttackReleased = false;
    }

    public void OnAttackEnd()
    {
        if (isAnimationFinished)
        {
            Check();
        }
        else
        {
            isAttackReleased = true;
        }          
    }

    public void BeginCheckAttackType()
    {
        if (isAttackReleased)
        {
            Check();
        }
        else
        {
            isAnimationFinished = true;
        }
    }

    public void Attack(int dmg, float range)
    {
        var enemies = Physics.OverlapSphere(forwardPos.position, range);       
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dir = enemies[i].transform.position - forwardPos.position;
            float angle = Vector3.Angle(forwardPos.forward, dir);

            if (enemies[i].GetComponent<EnemyBase>() && dir.magnitude <= range && angle < angleOfAttack )
            {
                enemies[i].GetComponent<EnemyBase>().TakeDamage(dmg, forwardPos.transform.forward);
            }
        }
    }
}

