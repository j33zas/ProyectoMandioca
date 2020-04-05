using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public float damage
    {
        get;
        private set;
    }
    protected float range;
    protected string name;
    protected float angleAttack;

    public Weapon(float dmg, float r, string n, float angle)
    {
        damage = dmg;
        range = r;
        name = n;
        angleAttack = angle;
    }

    public abstract EntityBase Attack(Transform pos);
}
