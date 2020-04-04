using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenericLifeSystem : MonoBehaviour
{
    CharacterLifeSystem lifeSystemEnemy;

    public FrontendStatBase uilife;

    public int life = 100;

    public event Action deadCallback = delegate { };

    public void AddEventOnDeath(Action listener) { deadCallback += listener; }
    public void RemoveEventOnDeath(Action listener) { deadCallback -= listener; deadCallback = delegate { }; }

    private void Start()
    {
        lifeSystemEnemy = new CharacterLifeSystem();
        lifeSystemEnemy.Config(life, EVENT_OnLoseLife, EVENT_OnGainLife, EVENT_OnDeath, uilife, life);
    }

    void EVENT_OnLoseLife() => Debug.Log("Enemy Lose life");
    void EVENT_OnGainLife() => Debug.Log("Enemy Gain life");
    void EVENT_OnDeath()
    {
        deadCallback.Invoke();
        deadCallback = delegate { };
    }

    public void Hit(int _val)
    {
        lifeSystemEnemy.Hit(_val);
    }
}
