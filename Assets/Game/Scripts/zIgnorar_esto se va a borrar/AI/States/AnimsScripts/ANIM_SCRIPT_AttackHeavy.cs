using UnityEngine;

public class ANIM_SCRIPT_AttackHeavy : ANIM_SCRIPT_Base 
{
    protected override void OnAnimationExit(Animator anim)
    {
        anim.SetBool("HeavyAttack", false);
    }
}
