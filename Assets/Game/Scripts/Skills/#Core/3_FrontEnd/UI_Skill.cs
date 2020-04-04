using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Skill : UI_ItemBase
{
    SkillInfo skillinfo;

    protected override void BeginFeedback()
    {
        SetImage(skillinfo.img_actived);
    }

    protected override void EndFeedback()
    {
        SetImage(skillinfo.img_avaliable);
    }

    public void Refresh()
    {
        SetImage(skillinfo.img_avaliable);
    }

    public void Set_SkillInfo(SkillInfo s) => skillinfo = s;
    public SkillInfo Get_SkilInfo() => skillinfo;
}
