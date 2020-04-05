using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleWeaponThree : Weapon
{
    public ExampleWeaponThree(float dmg, float r, string n, float angle) : base(dmg, r, n, angle)
    {
    }

    public override EntityBase Attack(Transform pos)
    {
        throw new System.NotImplementedException();
    }
}
