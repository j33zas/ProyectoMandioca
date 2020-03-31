using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    float range;
    public Transform forwardPos;

    float heavyAttackTime = 1f;
    float buttonTime;

    private void Update()
    {
        buttonTime += Time.deltaTime;
    }

    public void OnattackBegin()
    {
        buttonTime = 0f;
        Attack(2, 5f);

        //animacion
    }

    
    public void OnAttackEnd()
    {
        if (buttonTime < heavyAttackTime)
        {
            Debug.Log("Light Attack");
        }
        else
        {
            Debug.Log("Heavy Attack");
        }
    }
    //triggereado por animacion
    void Attack(int dmg, float range)
    {
        RaycastHit hit;
        if (Physics.Raycast(forwardPos.transform.position, forwardPos.transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.GetComponent<EnemyBase>())
                hit.collider.gameObject.GetComponent<EnemyBase>().TakeDamage(dmg);

        }
        Debug.DrawRay(forwardPos.transform.position, forwardPos.transform.forward, Color.black, range);
    }
}

