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

    CharacterAnimator anim;

    bool inCheck;

    Action NormalAttack;
    Action HeavyAttack;

    bool isAttackReleased;

    bool isAnimationFinished;
    ParticleSystem feedbackHeavy;

    bool oneshot;

    public bool inAttack;


    public CharacterAttack(float _range, float _heavyAttackTime, CharacterAnimator _anim, Transform _forward, Action _normalAttack, Action _heavyAttack, ParticleSystem ps)
    {
        range = _range;
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

    void CHECK()
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
       // feedbackHeavy.gameObject.SetActive(false);
        oneshot = false;
        buttonPressedTime = 0f;
        isAnimationFinished = false;
        isAttackReleased = false;
    }

    //SUELTO
    public void OnAttackEnd()
    {
        if (isAnimationFinished)
        {
            //quiere decir que ya llegue a la animacion
            CHECK();

        }
        else
        {
            //quiere decir que solte antes
            isAttackReleased = true;
        }
       
        
    }
    //ESTOY ARRIBA
    public void BeginCheckAttackType()
    {
        if (isAttackReleased)
        {
            //quiere decir que ya solte
            CHECK();
        }
        else
        {
            //quiere decir que todavia no solte
            isAnimationFinished = true;
        }
    }

    //triggereado por animacion
    public void Attack(int dmg, float range)
    {
        RaycastHit hit;
        if (Physics.Raycast(forwardPos.transform.position, forwardPos.transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.GetComponent<EnemyBase>())
            {
                hit.collider.gameObject.GetComponent<EnemyBase>().TakeDamage(dmg);
            }
        }
        Debug.DrawRay(forwardPos.transform.position, forwardPos.transform.forward, Color.black, range);
    }
}

