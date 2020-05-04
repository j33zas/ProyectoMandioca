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

    public Action<bool> OnAttack;
    public Action OnAttackBegin;
    public Action OnAttackEnd;

    ParticleSystem feedbackHeavy;
    bool oneshot;

    public bool inAttack;

    //FirstAttackPassive
    private bool firstAttack;

    List<Weapon> myWeapons;
    public Weapon currentWeapon { get; private set; }
    int currentIndexWeapon;

    Action callback_ReceiveEntity = delegate { };

    event Action<Vector3> callbackPositio;

    ParticleSystem attackslash;

    Action DealSuccesfullNormal;
    Action DealSuccesfullHeavy;


    public CharacterAttack(float _range, float _angle, float _heavyAttackTime, CharacterAnimator _anim, Transform _forward, Action _normalAttack, Action _heavyAttack, ParticleSystem ps, float damage, ParticleSystem _attackslash)
    {
        myWeapons = new List<Weapon>();
        myWeapons.Add(new GenericSword(damage, _range, "Generic Sword", _angle));
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

        OnAttack += Attack;
        OnAttackBegin += AttackBegin;
        OnAttackEnd += AttackEnd;

        attackslash = _attackslash;
    }



    public string ChangeName()
    {
        return currentWeapon.weaponName;
    }

    public void BeginFeedbackSlash() => attackslash.Play();
    public void EndFeedbackSlash() => attackslash.Stop();

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

    void AttackBegin()
    {
        inCheck = true;
        buttonPressedTime = 0f;
        anim.OnAttackBegin(true);
    }

    public void AttackFail()
    {
        inCheck = false;
        buttonPressedTime = 0f;
        anim.OnAttackBegin(false);
        feedbackHeavy.Stop();
        oneshot = false;
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
        anim.OnAttackBegin(false);
    }

    //input
    void AttackEnd()
    {
        Check();
   
    }

    //anim espada arriba
    public void BeginCheckAttackType()
    {
        Main.instance.Vibrate();
    }

    public void ChangeDamageBase(int dmg) => currentDamage = dmg;

    public void ConfigureDealsSuscessful(Action inNormal, Action inHeavy)
    {
        DealSuccesfullNormal = inNormal;
        DealSuccesfullHeavy = inHeavy;
    }
    void Attack(bool isHeavy)
    {
        var sucessful = currentWeapon.Attack(forwardPos, currentDamage);
        attackslash.Play();

        if (sucessful)
        {
            callback_ReceiveEntity();
            if (isHeavy) DealSuccesfullHeavy();
            else DealSuccesfullNormal();
        }

        FirstAttackReady(false);
    }

    public void AddCAllback_ReceiveEntity(Action _cb) => callback_ReceiveEntity += _cb;
    public void RemoveCAllback_ReceiveEntity(Action _cb)
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

