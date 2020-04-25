using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyCC_RaycastMetod : CombatComponent
{
    [Header("Raycast Method")]
    [SerializeField] LayerMask _lm;
    [SerializeField] float distance;
   
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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (Main.instance.GetChar().transform.position - transform.position), out hit, distance, _lm))
        {
            showray = true;

            if (hit.collider.GetComponent<EntityBase>())
            {
                EntityBase character = hit.collider.GetComponent<EntityBase>();
                callback.Invoke(character);
            }
        }
        else
        {
            showray = false;
        }
    }

    
}
