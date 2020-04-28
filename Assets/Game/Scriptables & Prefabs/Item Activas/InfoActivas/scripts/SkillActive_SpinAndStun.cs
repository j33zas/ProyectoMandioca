using UnityEngine;
public class SkillActive_SpinAndStun : SkillActivas
{
    [Header("Spin And Stun Settings")]
    [SerializeField] float spinDuration;
    [SerializeField] float spinSpeed;
    [SerializeField] float stunDuration = 3f;
    [SerializeField] int damage = 5;
    

    protected override void OnOneShotExecute() { Main.instance.GetChar().StartSpin(spinDuration, spinSpeed, stunDuration); }
    #region en desuso
    protected override void OnBeginSkill()
    {
        
    }
    
    protected override void OnEndSkill() { }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateSkill() { }
    protected override void OnUpdateUse() { }
    #endregion
}
