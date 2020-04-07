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
        enemies = FindObjectsOfType<DummyEnemy>().ToList();

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
    }

    protected override void OnUpdateSkill()
    {

    }

    public void ReceiveFocusOnParry(Vector3 pos, FocusOnParryComponent component)
    {
        foreach (var item in focusOnParryComponents)
        {
            Minion myMinion = item.GetComponent<Minion>();

            for (int i = 0; i < enemies.Count; i++)
            {
                /*if(enemies[i].isTarget == true)
                {
                }*/
            }
        }
    }
}
