using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillPetrify : SkillBase
{
    List<PetrifyComponent> petrifyComponents = new List<PetrifyComponent>();
    public float petrifyRange = 10;

    public override void OnBeginSkill()
    {
        petrifyComponents = new List<PetrifyComponent>();
        petrifyComponents = FindObjectsOfType<PetrifyComponent>().ToList();

        foreach (var item in petrifyComponents)
        {
            if (item != null)
            {
                item.Configure(ReceivePetrifyOnDeathMinion);
                item.OnBegin();
            }
        }
    }

    public override void OnEndSkill()
    {
        foreach (var item in petrifyComponents)
        {
            if(item!= null) item.OnEnd();
        }
    }

    public override void OnUpdateSkill()
    {
    }

    public void ReceivePetrifyOnDeathMinion(Vector3 pos, PetrifyComponent p)
    {
        var listOfEntities = Physics.OverlapSphere(pos, petrifyRange );

        petrifyComponents.Remove(p);

        foreach (var item in listOfEntities)
        {
            EnemyBase myEnemy = item.GetComponent<EnemyBase>();
            if (myEnemy)
            {
                myEnemy.Petrified();
            }
        }
    }
}
