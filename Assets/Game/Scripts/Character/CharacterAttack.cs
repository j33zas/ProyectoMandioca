using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterAttack
{


    public Transform forwardPos { get; private set; }
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
    public Weapon currentWeapon { get; private set; }
    int currentIndexWeapon;

    Action<EnemyBase> callback_ReceiveEntity = delegate { };

    event Action<Vector3> callbackPosition;


    public CharacterAttack(float _range, float _angle, float _heavyAttackTime, CharacterAnimator _anim, Transform _forward, Action _normalAttack, Action _heavyAttack, ParticleSystem ps, float rangeOfPetrified, float damage)
    {
        myWeapons = new List<Weapon>();
        myWeapons.Add(new GenericSword(damage, _range, "Generic Sword", 45));
        myWeapons.Add(new ExampleWeaponOne(damage, _range, "Other Weapon", 45));
        myWeapons.Add(new ExampleWeaponTwo(damage, _range, "Sarasa Weapon", 45));
        myWeapons.Add(new ExampleWeaponThree(damage, _range, "Ultimate Blessed Weapon", 45));
        currentWeapon = myWeapons[0];
        currentDamage = currentWeapon.baseDamage;

        heavyAttackTime = _heavyAttackTime;
        anim = _anim;
        forwardPos = _forward;

        NormalAttack = _normalAttack;
        HeavyAttack = _heavyAttack;
        feedbackHeavy = ps;
        _rangeOfPetrified = rangeOfPetrified;
    }

    public string ChangeName()
    {
        return currentWeapon.weaponName;
    }

    public void BuffOrNerfDamage(float f)
    {
        currentDamage += f;
    }

    public void ChangeWeapon(int index)
    {
        currentIndexWeapon += index;

        if (currentIndexWeapon >= myWeapons.Count)
        {
            currentIndexWeapon = 0;
        }
        else if (currentIndexWeapon < 0)
        {
            currentIndexWeapon = myWeapons.Count - 1;
        }

        currentDamage -= currentWeapon.baseDamage;
        currentWeapon = myWeapons[currentIndexWeapon];
        currentDamage += currentWeapon.baseDamage;
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
        EntityBase enemy = currentWeapon.Attack(forwardPos, currentDamage);

        if (enemy != null)
        {
            if (enemy.GetComponent<EnemyBase>())
            {
                callback_ReceiveEntity((EnemyBase)enemy);
            }
        }

        FirstAttackReady(false);
    }

    public void AddCAllback_ReceiveEntity(Action<EnemyBase> _cb) => callback_ReceiveEntity += _cb;
    public void RemoveCAllback_ReceiveEntity(Action<EnemyBase> _cb)
    {
        callback_ReceiveEntity -= _cb;
        callback_ReceiveEntity = delegate { };
    }

    public void ActiveFirstAttack() => firstAttack = true;
    public void DeactiveFirstAttack() => firstAttack = false;
    public bool IsFirstAttack() => firstAttack;

    public void FirstAttackReady(bool ready)
    {
        firstAttack = ready;
    }
}

