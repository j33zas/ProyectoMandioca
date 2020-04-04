using UnityEngine;
public abstract class SkillBase : MonoBehaviour
{
    bool canupdate = false;
    public SkillInfo skillinfo;
    public void BeginSkill() { canupdate = true; OnBeginSkill(); }
    public void EndSkill() { canupdate = false; OnEndSkill(); }
    private void Update() { if (canupdate) OnUpdateSkill(); }
    protected abstract void OnBeginSkill();
    protected abstract void OnEndSkill();
    protected abstract void OnUpdateSkill();
}
