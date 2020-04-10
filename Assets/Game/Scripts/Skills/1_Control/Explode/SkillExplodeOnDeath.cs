using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillExplodeOnDeath : SkillBase
{
    List<ExplodeComponent> explodeComponents = new List<ExplodeComponent>();
    public float explosionRange = 10;
    public int explosionDmg = 20;

    protected override void OnBeginSkill()
    {
        explodeComponents = new List<ExplodeComponent>();
        explodeComponents = FindObjectsOfType<ExplodeComponent>().ToList();

        foreach (var item in explodeComponents)
        {
            if (item != null)
            {
                item.Configure(ReceiveExplodeOnDeath);
                item.OnBegin();
            }
        }
    }

    protected override void OnEndSkill()
    {
        foreach (var item in explodeComponents)
        {
            if (item != null) item.OnEnd();
        }
    }

    protected override void OnUpdateSkill()
    {
    }

    public void ReceiveExplodeOnDeath(Vector3 pos, ExplodeComponent p)
    {
        var listOfEntities = Physics.OverlapSphere(pos, explosionRange);

        explodeComponents.Remove(p);

        foreach (var item in listOfEntities)
        {
            EnemyBase myEnemy = item.GetComponent<EnemyBase>();
            if (myEnemy)
            {
                Debug.Log("0: explosionDmg: " + explosionDmg);
                myEnemy.TakeDamage(explosionDmg, Vector3.up, Damagetype.explosion);
            }
        }
    }
}
