using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public float baseDamage
    {
        get;
        private set;
    }
    protected float range;
    protected float originalRange;
    public string weaponName
    {
        get;
        private set;
    }
    protected float angleAttack;

    protected CharacterHead _head;

    public Weapon(float dmg, float r, string n, float angle)
    {
        baseDamage = dmg;
        range = r;
        weaponName = n;
        angleAttack = angle;
        originalRange = range;
        _head = Main.instance.GetChar();
    }

    /// <summary>
    /// SinParametro vuelve al rangoOriginal
    /// </summary>
    public float ModifyAttackrange()
    {
        range = originalRange;
        
        return range;
    }
    /// <summary>
    /// Cambia a un rango X que se le mande
    /// </summary>
    /// <param name="changedValue"></param>
    public float ModifyAttackrange(float changedValue)
    {
        range = changedValue;

        return range;
    }

    public float GetWpnRange()
    {
        return range;
    }
    public abstract EntityBase Attack(Transform pos, float damage);
}
