using UnityEngine;
public abstract class SkillBase : MonoBehaviour
{
    bool canupdate = false;
    public SkillInfo skillinfo;
    UI_Skill ui_skill;
    public void SetUI(UI_Skill _ui) => ui_skill = _ui;
    public void BeginSkill()
    {
        canupdate = true;
        OnBeginSkill();
        ui_skill.OnUI_Select();
    }
    public void EndSkill()
    {
        canupdate = false;
        OnEndSkill();
        ui_skill.OnUI_Unselect();
    }
    private void Update() { if (canupdate) OnUpdateSkill(); }
    protected abstract void OnBeginSkill();
    protected abstract void OnEndSkill();
    protected abstract void OnUpdateSkill();
}
