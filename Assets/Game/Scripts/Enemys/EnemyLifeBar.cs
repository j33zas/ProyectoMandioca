using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeBar : MonoBehaviour
{
    CharacterLifeSystem lifeSystemEnemy;
    
    public FrontendStatBase uilife;

    public int life = 100;

    private void Start()
    {
        lifeSystemEnemy = new CharacterLifeSystem();
        lifeSystemEnemy.Config(life, EVENT_OnLoseLife, EVENT_OnGainLife, EVENT_OnDeath, uilife, life);
    }

    void EVENT_OnLoseLife() => Debug.Log("Enemy Lose life");
    void EVENT_OnGainLife() => Debug.Log("Enemy Gain life");
    void EVENT_OnDeath() => Debug.Log("Enemy Death");

    public void Hit(int _val)
    {
        lifeSystemEnemy.Hit(_val);
    }
}
