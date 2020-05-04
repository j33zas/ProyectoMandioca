using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyCC_RaycastMetod : CombatComponent
{
    [Header("Overlap")]
    [SerializeField] LayerMask _lm;
    [SerializeField] float distance;
    [SerializeField] float angleAttack = 45;
    [SerializeField] Transform rot;
   
    bool showray;

    public override void ManualTriggerAttack() 
    {
        Calculate();
    }
    public override void BeginAutomaticAttack() 
    { 

    }

    private void OnDrawGizmos()
    {
        //if (showray) Gizmos.color = Color.red;
        //else Gizmos.color = Color.green;
            
        //    Gizmos.DrawRay(transform.position, (target.position - transform.position));
    }
    public override void Play()
    {
        
    }

    public override void Stop()
    {
        showray = false;
    }

    void Calculate()
    {
        EntityBase entity = null;

        var enemies = Physics.OverlapSphere(rot.position, distance, _lm);
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dir = enemies[i].transform.position - rot.position;
            float angle = Vector3.Angle(rot.forward, dir);

            if (enemies[i].GetComponent<EntityBase>() && dir.magnitude <= distance && angle < angleAttack)
            {
                if (entity == null)
                    entity = enemies[i].GetComponent<EntityBase>();
            }
        }

        if(entity!=null)
            callback.Invoke(entity);
    }

    
}
