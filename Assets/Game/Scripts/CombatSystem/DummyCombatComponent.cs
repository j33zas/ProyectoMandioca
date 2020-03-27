using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCombatComponent : CombatComponent
{
    [SerializeField] LayerMask _lm;
    [SerializeField] float distance;
    public Transform target;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (target.position - transform.position), out hit, distance, _lm))
            {
                if (hit.collider.GetComponent<EntityBase>())
                {
                    EntityBase character = hit.collider.GetComponent<EntityBase>();
                    callback.Invoke(character);
                }
            }
        }
    }
}
