using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillFocusOnParry : SkillBase
{

    List<FocusOnParryComponent> focusOnParryComponents = new List<FocusOnParryComponent>();



    protected override void OnBeginSkill()
    {
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
            myMinion.ChangeToAttackState();
        }
    }
}
