using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public void OnattackBegin()
    {
        Debug.Log("OnAttackBegin");
    }
    public void OnAttackEnd()
    {
        Debug.Log("OnAttackBegin");
    }

    void RecibiUnEnemigo(EntityBase ebase)
    {
        ebase.TakeDamage(13213);
    }
}

