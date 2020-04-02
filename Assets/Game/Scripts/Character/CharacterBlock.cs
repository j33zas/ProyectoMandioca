using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterBlock : EntityBlock
{
    Action beginparry;
    Action endparry;

    CharacterAnimator anim;

    public CharacterBlock(float timeParry, float blockRange, Action _EndParry, CharacterAnimator _anim) : base(timeParry, blockRange)
    {
        endparry = _EndParry;
        anim = _anim;
    }

    public override void OnBlockDown() { anim.Block(true); }
    public override void OnBlockUp() { OnBlockUpSuccessful(); anim.Block(false); }

    public override void FinishParry()
    {
        base.FinishParry();
        endparry.Invoke();
    }
}
