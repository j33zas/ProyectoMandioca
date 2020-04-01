using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test_Attack : MonoBehaviour
{
    [SerializeField]
    LayerMask _lm;
    [SerializeField]
    float distance;
    public Transform target;
    Action attack;
    [SerializeField]
    int damage;
    StatesMachine sm;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        attack += Attack;
        sm = new StatesMachine();
        sm.Addstate(new StatesFollow(sm, transform, GetComponent<Rigidbody>(), FindObjectOfType<CharacterHead>().transform, anim, 50, 3));
        sm.Addstate(new StatesAttack(sm, anim, transform, FindObjectOfType<CharacterHead>().transform, 50, 3));
        sm.ChangeState<StatesAttack>();

    }

    // Update is called once per frame
    void Update()
    {
        sm.Update();
        if (Input.GetKeyDown(KeyCode.F))
        {
            attack();
        }
    }

    void Attack()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,(target.position-transform.position),out hit, distance, _lm))
        {
            if (hit.collider.GetComponent<EntityBase>())
            {
                EntityBase character = hit.collider.GetComponent<EntityBase>();
                character.TakeDamage(damage);
            }
        }
    }
}
