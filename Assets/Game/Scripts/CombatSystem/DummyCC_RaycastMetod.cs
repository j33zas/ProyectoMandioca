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
    public Transform target;

    bool automatic_attack;
    float timer;

    public GameObject feedbackattack;
    PopSignalFeedback popsignal_attack;
    bool showray;

   [SerializeField] Image feedback_timer_attack;

    private void Awake()
    {
        automatic_attack = true;
        popsignal_attack = new PopSignalFeedback(0.1f,feedbackattack);
    }
    public override void ManualTriggerAttack() => automatic_attack = false;
    public override void BeginAutomaticAttack() => automatic_attack = true;

    private void OnDrawGizmos()
    {
        if (showray) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
            
            Gizmos.DrawRay(transform.position, (target.position - transform.position));
    }
    public override void Play()
    {
        timer = 0;
        automatic_attack = true;
    }

    public override void Stop()
    {
        timer = 0;
        automatic_attack = false;
        showray = false;
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
                        
                        popsignal_attack.Show();
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

        feedback_timer_attack.fillAmount = timer;

        popsignal_attack.Refresh();
    }

    
}
