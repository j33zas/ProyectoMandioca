using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillFocusOnParry : SkillBase
{

    List<FocusOnParryComponent> focusOnParryComponents = new List<FocusOnParryComponent>();
    List<DummyEnemy> enemies = new List<DummyEnemy>();


    protected override void OnBeginSkill()
    {
        enemies = new List<DummyEnemy>();
        enemies = Main.instance.GetEnemies();

        foreach (var item in enemies)
        {
            if(item != null)
            {   
                item.OnParried += item.getFocusedOnParry;
            }
        }

        focusOnParryComponents = new List<FocusOnParryComponent>();
        focusOnParryComponents = FindObjectsOfType<FocusOnParryComponent>().ToList();

        foreach (var item in focusOnParryComponents)
        {
            item.Configure(ReceiveFocusOnParry);
            item.OnBegin();
        }
    }

    protected override void OnEndSkill()
    {
        foreach (var item in focusOnParryComponents)
        {
            if (item != null) item.OnEnd();
        }
        foreach (var item in enemies)
        {
            if (item != null)
            {
                item.OnParried -= item.getFocusedOnParry;
                item.isTarget = false;
            }
        }
    }

    protected override void OnUpdateSkill()
    {

    }

    public void ReceiveFocusOnParry(Vector3 pos, FocusOnParryComponent component)
    {
        foreach (var item in focusOnParryComponents)
        {
            Minion myMinion = item.GetComponent<Minion>();

            foreach (var enemy in enemies)
            {
                if(enemy.isTarget)
                {
                    myMinion.ChangeToAttackState(enemy.transform);
                }
            }
        }
    }
}
