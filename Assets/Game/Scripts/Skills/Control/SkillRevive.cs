using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SkillRevive : SkillBase
{
    List<ReviveComponent> enemiesThatCanRevive = new List<ReviveComponent>();
    
    protected override void OnBeginSkill()
    {
        enemiesThatCanRevive = new List<ReviveComponent>();
        //enemiesCanRevive = Main.instance.GetListOf<ReviveComponent>();
        enemiesThatCanRevive = FindObjectsOfType<ReviveComponent>().ToList();

        for (int i = 0; i < enemiesThatCanRevive.Count; i++)
        {
            print(enemiesThatCanRevive[i]);
        }
        
        foreach (var item in enemiesThatCanRevive)
        {
            if (item != null)
            {
                item.Configure(SpawnMinionOnEnemyDeath);
                item.OnBegin();
            }
        }
    }

    protected override void OnEndSkill()
    {
        foreach (var item in enemiesThatCanRevive)
        {
            if (item != null) item.OnEnd();
        }
    }
    protected override void OnUpdateSkill() { }

    void SpawnMinionOnEnemyDeath(Vector3 pos, ReviveComponent enemy, GameObject prefab)
    {
        var myMinion = GameObject.Instantiate(prefab);
        myMinion.transform.position = pos;

    }

}
