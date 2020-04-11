using UnityEngine;
public abstract class SkillBase : MonoBehaviour
{
    bool canupdate = false;
    public SkillInfo skillinfo;
    protected UI_Skill ui_skill;
    public void SetUI(UI_Skill _ui) => ui_skill = _ui;
    bool alreadyActived;
    public virtual void BeginSkill()
    {
        if (!alreadyActived)
        {
            alreadyActived = true;
            canupdate = true;
            if(ui_skill) ui_skill.OnUI_Select();
            OnBeginSkill();
        }
    }
    public virtual void EndSkill()
    {
        alreadyActived = false;
        canupdate = false;
        if (ui_skill) ui_skill.OnUI_Unselect();
        OnEndSkill();
    }
    private void Update() { absUpdate(); }
    internal virtual void absUpdate() { if (canupdate) OnUpdateSkill(); }
    protected abstract void OnBeginSkill();
    protected abstract void OnEndSkill();
    protected abstract void OnUpdateSkill();
}
