using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterAttack
{
    Transform forwardPos;
    float heavyAttackTime = 1f;
    float buttonPressedTime;
    float angleOfAttack;
    float currentDamage;

    CharacterAnimator anim;

    bool inCheck;

    Action NormalAttack;
    Action HeavyAttack;
    
    bool isAttackReleased;
    bool isAnimationFinished;
    ParticleSystem feedbackHeavy;
    bool oneshot;

    public bool inAttack;

    //FirstAttackPassive
    private bool pasiveFirstAttack;
    private bool firstAttack;
    private float _rangeOfPetrified;

    List<Weapon> myWeapons;
    Weapon currentWeapon;
    int currentIndexWeapon;


    public CharacterAttack(float _range, float _angle, float _heavyAttackTime, CharacterAnimator _anim, Transform _forward, Action _normalAttack, Action _heavyAttack, ParticleSystem ps,float rangeOfPetrified, float damage)
    {
        myWeapons = new List<Weapon>();
        myWeapons.Add(new GenericSword(damage, _range, "Generic Sword", 45));
        myWeapons.Add(new ExampleWeaponOne(damage, _range, "Other Weapon", 45));
        myWeapons.Add(new ExampleWeaponTwo(damage, _range, "Sarasa Weapon", 45));
        myWeapons.Add(new ExampleWeaponThree(damage, _range, "Ultimate Blessed Weapon", 45));
        currentWeapon = myWeapons[0];
        currentDamage = currentWeapon.damage;

        heavyAttackTime = _heavyAttackTime;
        anim = _anim;
        forwardPos = _forward;

        NormalAttack = _normalAttack;
        HeavyAttack = _heavyAttack;
        feedbackHeavy = ps;
        _rangeOfPetrified = rangeOfPetrified;
    }

    public void ChangeWeapon()
    {

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

    public void Attack()
    {
        EntityBase entity = currentWeapon.Attack(forwardPos);
        if (entity)
        {
            if (pasiveFirstAttack)
            {
                FirstAttack(entity.transform);
            }
        }
        FirstAttackReady(false);
    }

    void FirstAttack(Transform enemyPosition)
    {
        if (firstAttack)
        {
            PetrifiedEnemy(enemyPosition);           
        }
    }

    void PetrifiedEnemy(Transform enemyPosition)
    {
        var listOfEnemy = Physics.OverlapSphere(enemyPosition.position, _rangeOfPetrified);
        foreach (var item in listOfEnemy)
        {
            EnemyBase myEnemy = item.GetComponent<EnemyBase>();
            if (myEnemy)
            {
                myEnemy.Petrified();
            }
        }
    }

    public void FirstAttackReady(bool ready)
    {
        firstAttack = ready;
    }
    public void PasiveFirstAttackReady(bool ready)
    {
        pasiveFirstAttack = ready;
    }
}

