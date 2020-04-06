using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenericLifeSystem : MonoBehaviour
{
    CharacterLifeSystem lifeSystemEnemy;

    public FrontendStatBase uilife;

    public int life = 100;

    bool isdeath;

    public event Action DeadCallback = delegate { };

    public void AddEventOnDeath(Action listener) { DeadCallback += listener; }
    public void RemoveEventOnDeath(Action listener) { DeadCallback -= listener; DeadCallback = delegate { }; }

    private void Start()
    {
        uilife = GetComponentInChildren<LifeBar>(); 
        lifeSystemEnemy = new CharacterLifeSystem();
        lifeSystemEnemy.Config(life, EVENT_OnLoseLife, EVENT_OnGainLife, EVENT_OnDeath, uilife, life);
    }

    void EVENT_OnLoseLife() => Debug.Log("Enemy Lose life");
    void EVENT_OnGainLife() => Debug.Log("Enemy Gain life");
    void EVENT_OnDeath()
    {
        if (!isdeath)
        {
            DeadCallback.Invoke();
            DeadCallback = delegate { };
            isdeath = true;
        }
    }

    public void Hit(int _val)
    {
        lifeSystemEnemy.Hit(_val);
    }
}
