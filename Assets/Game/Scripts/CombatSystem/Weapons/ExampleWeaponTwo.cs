using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleWeaponTwo : Weapon
{
    public ExampleWeaponTwo(float dmg, float r, string n, float angle) : base(dmg, r, n, angle)
    {
    }

    public override EntityBase Attack(Transform pos, float damage)
    {
        EntityBase entity = null;

        var enemies = Physics.OverlapSphere(pos.position, range);
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dir = enemies[i].transform.position - pos.position;
            float angle = Vector3.Angle(pos.forward, dir);

            if (enemies[i].GetComponent<EnemyBase>() && dir.magnitude <= range && angle < angleAttack)
            {
                if (entity == null)
                    entity = enemies[i].GetComponent<EntityBase>();

                enemies[i].GetComponent<EnemyBase>().TakeDamage((int)damage, pos.forward, Damagetype.parriable, _head);
            }
        }

        return entity;
    }

}
