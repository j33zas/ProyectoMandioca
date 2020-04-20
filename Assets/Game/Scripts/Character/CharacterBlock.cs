using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class CharacterBlock : EntityBlock
{
    Action beginparry;
    Action endparry;

    public Action OnBlock;
    public Action UpBlock;
    public Action OnParry;

    CharacterAnimator anim;
    GameObject feedback;

    Func<EventStateMachine<CharacterHead.PlayerInputs>> sm;


    public CharacterBlock(float timeParry,
                          float blockRange,
                          Action _EndParry,
                          CharacterAnimator _anim,
                          GameObject feedbackBlock,
                          Func<EventStateMachine<CharacterHead.PlayerInputs>> _sm) : base(timeParry, blockRange)
    {


        endparry = _EndParry;
        anim = _anim;
        feedback = feedbackBlock;
        OnBlock += OnBlockDown;
        UpBlock += OnBlockUp;
        sm = _sm;

        OnParry += Parry;
    }

    public override void OnBlockDown() { anim.Block(true); }
    public override void OnBlockUp() { anim.Block(false); }

    //por animacion
    public override void OnBlockSuccessful()
    {
        sm().SendInput(CharacterHead.PlayerInputs.BLOCK);
    }

    public void SetOnBlock(bool b)
    {
        feedback.SetActive(b);
        onBlock = b;
    }

    public override void FinishParry()
    {
        base.FinishParry();
        endparry.Invoke();
    }
}
