using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillExplodeOnDeath : SkillBase
{
    List<ExplodeComponent> petrifyComponents = new List<ExplodeComponent>();
    public float explosionRange = 10;
    public int explosionDmg = 20;

    protected override void OnBeginSkill()
    {
        petrifyComponents = new List<ExplodeComponent>();
        petrifyComponents = FindObjectsOfType<ExplodeComponent>().ToList();

        foreach (var item in petrifyComponents)
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
        foreach (var item in petrifyComponents)
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



        petrifyComponents.Remove(p);

        foreach (var item in listOfEntities)
        {
            EnemyBase myEnemy = item.GetComponent<EnemyBase>();
            if (myEnemy)
            {
                Vector3 dir = myEnemy.transform.position - pos;
                dir.Normalize();

                myEnemy.TakeDamage(explosionDmg, dir, Damagetype.explosion);
            }
        }
    }
}
