using UnityEngine;
public abstract class SkillBase : MonoBehaviour
{
    bool canupdate = false;
    public void BeginSkill() { canupdate = true; OnBeginSkill(); }
    public void EndSkill() { canupdate = false; OnEndSkill(); }
    private void Update() { if (canupdate) OnUpdateSkill(); }
    public abstract void OnBeginSkill();
    public abstract void OnEndSkill();
    public abstract void OnUpdateSkill();
}
