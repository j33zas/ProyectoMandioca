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
        if (!isdeath)
        {
            deadCallback.Invoke();
            deadCallback = delegate { };
            isdeath = true;
        }
    }

    public void Hit(int _val)
    {
        lifeSystemEnemy.Hit(_val);
    }

    public void DoTSystem(float duration, float timePerTick, int tickDamage, Damagetype damagetype, Action onFinishCallback )
    {
        StartCoroutine(DoT(duration, timePerTick, tickDamage, damagetype, onFinishCallback));
    }

    IEnumerator DoT(float duration, float timePerTick, int tickDamage, Damagetype damagetype, Action onFinishCallback)
    {
        float countTime = 0;
        float tickTimeCount = 0;
        
        while (countTime <= duration)
        {
            countTime += Time.fixedDeltaTime;

            tickTimeCount += Time.fixedDeltaTime;

            if (tickTimeCount >= timePerTick)
            {
                Hit(tickDamage);
                tickTimeCount = 0;
            }

            yield return null;
        }
        
        onFinishCallback.Invoke();
    }
}
