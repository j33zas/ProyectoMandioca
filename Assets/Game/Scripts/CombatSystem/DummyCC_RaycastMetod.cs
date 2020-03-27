using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCC_RaycastMetod : CombatComponent
{
    [Header("Raycast Method")]
    [SerializeField] LayerMask _lm;
    [SerializeField] float distance;
    public Transform target;

    bool automatic_attack;
    float timer;

    public GameObject feedbackattack;
    PopSignalFeedback popsignal;
    bool showray;

    private void Awake()
    {
        automatic_attack = true;

        popsignal = new PopSignalFeedback(0.1f,feedbackattack);
    }
    public override void ManualTriggerAttack() => automatic_attack = false;
    public override void BeginAutomaticAttack() => automatic_attack = true;

    private void OnDrawGizmos()
    {
        if (showray) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
            
            Gizmos.DrawRay(transform.position, (target.position - transform.position));
    }

    void Update()
    {
        if (automatic_attack)
        {
            if (timer < 1)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, (target.position - transform.position), out hit, distance, _lm))
                {
                    showray = true;

                    if (hit.collider.GetComponent<EntityBase>())
                    {
                        
                        popsignal.Show();
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

        popsignal.Refresh();
    }
}
