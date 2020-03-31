using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack
{
    Transform forwardPos;
    float range;
    float heavyAttackTime = 1f;
    float buttonPressedTime;

    CharacterAnimator anim;

    public CharacterAttack(float _range, float _heavyAttackTime, CharacterAnimator _anim, Transform _forward)
    {
        range = _range;
        heavyAttackTime = _heavyAttackTime;
        anim = _anim;
        forwardPos = _forward;
    }

    private void Update()
    {
        buttonPressedTime += Time.deltaTime;
    }

    public void OnattackBegin()
    {
        buttonPressedTime = 0f;
        anim.OnAttackBegin();
    }


    public void OnAttackEnd()
    {
        if (buttonPressedTime < heavyAttackTime)
        {
            Debug.Log("Light Attack");
        }
        else
        {
            Debug.Log("Heavy Attack");
        }
    }
    //triggereado por animacion
    public void Attack(int dmg, float range)
    {
        RaycastHit hit;
        if (Physics.Raycast(forwardPos.transform.position, forwardPos.transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.GetComponent<EnemyBase>())
            {
                hit.collider.gameObject.GetComponent<EnemyBase>().TakeDamage(dmg);
                Debug.Log("Hit Enemy");
            }
        }
        Debug.DrawRay(forwardPos.transform.position, forwardPos.transform.forward, Color.black, range);
    }
}

