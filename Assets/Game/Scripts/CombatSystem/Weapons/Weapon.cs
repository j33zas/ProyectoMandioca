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
    protected float angle;
    protected float originalAngle;

    protected CharacterHead _head;

    public Weapon(float dmg, float r, string n, float angle)
    {
        baseDamage = dmg;
        range = r;
        weaponName = n;
        this.angle = angle;
        originalAngle = angle;
        originalRange = range;
        _head = Main.instance.GetChar();
    }

    /// está por parametro opcional, si se envia parametro usa ese valor, si no se manada setea -1 y usa el rango original
    public float ModifyAttackrange(float changedValue = -1) 
    { 
        range = (changedValue == -1) ? range = originalRange : range = changedValue; 
        return range; 
    }
    public float ModifyAttackAngle(float changedValue = -1)
    {
        angle = (changedValue == -1) ? angle = originalAngle : angle = changedValue;
        return angle;
    }

    public float GetWpnRange()
    {
        return range;
    }
    public abstract EntityBase Attack(Transform pos, float damage);
}
