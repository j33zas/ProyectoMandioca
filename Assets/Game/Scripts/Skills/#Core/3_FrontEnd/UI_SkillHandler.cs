using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillHandler : UI_Base
{
    Dictionary<SkillType, Transform> positions = new Dictionary<SkillType, Transform>();

    public void Build(List<SkillBase> skills)
    {
        foreach (var s in skills)
        {
            var parent = positions[s.skillinfo.skilltype];
            var info = s.skillinfo;

        }
    }

    public override void Refresh() { }
    protected override void OnAwake() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnEndOpenAnimation() { }
    protected override void OnStart() { }
    protected override void OnUpdate() { }
}
