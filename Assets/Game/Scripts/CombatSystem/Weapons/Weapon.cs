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
    public string weaponName
    {
        get;
        private set;
    }
    protected float angleAttack;

    public Weapon(float dmg, float r, string n, float angle)
    {
        baseDamage = dmg;
        range = r;
        weaponName = n;
        angleAttack = angle;
    }

    public abstract EntityBase Attack(Transform pos, float damage);
}
