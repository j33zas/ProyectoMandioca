using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EmptySkillActivas : SkillActivas
{
    protected override void OnBeginSkill() { }
    protected override void OnEndSkill() { }
    protected override void OnExecute() { Debug.Log("executeeeee"); }

    protected override void OnUpdateSkill() { }
}
