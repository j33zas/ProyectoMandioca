using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EXAMPLE_Enemy : EnemyBase
{
    Pinche pinche;

    Action<EntityBase> callback;

    public override void Awake()
    {
        base.Awake(); //no borrar

        /// bla bla bla
        /// 

        pinche.ConfigureDelegate(RecibiUnEnemigoParaDarleElDamage);
    }

    void RecibiUnEnemigoParaDarleElDamage(EntityBase e)
    {
        //aca tenes que chequear que sea el character controller
        e.TakeDamage(12321);//tobias
    }

    public override void TakeDamage(int dmg)//agustin
    {
        //recibir el daño del character controllers
    }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity() { }


}



public class Pinche : MonoBehaviour
{
    Action<EntityBase> attack;


    public void ConfigureDelegate(Action<EntityBase> bla)
    {
        attack += bla;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other == character)
        //{
        //    attack(other.cast);
        //}
    }
}
