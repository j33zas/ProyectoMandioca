using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.StateMachine;

public class CharacterBlock : EntityBlock
{
    public Action OnBlock;
    public Action UpBlock;
    public Action OnParry;

    public Action BeginParry;

    CharacterAnimator anim;

    ParticleSystem parryParticles;

    Func<EventStateMachine<CharacterHead.PlayerInputs>> sm;


    public CharacterBlock(float timeParry,
                          float blockRange,
                          CharacterAnimator _anim,
                          Func<EventStateMachine<CharacterHead.PlayerInputs>> _sm,
                          ParticleSystem _parryParticles) : base(timeParry, blockRange)
    {
        anim = _anim;
        OnBlock += OnBlockDown;
        UpBlock += OnBlockUp;
        sm = _sm;
        parryParticles = _parryParticles;
        OnParry += FinishParry;
        BeginParry += Parry;
        BeginParry += ParryFeedback;
    }

    public override void OnBlockDown() { anim.Block(true); }
    public override void OnBlockUp() { anim.Block(false); FinishParry(); }

    //por animacion
    public override void OnBlockSuccessful()
    {
        sm().SendInput(CharacterHead.PlayerInputs.BLOCK);
        BeginParry();
    }

    void ParryFeedback()
    {
        parryParticles.Play();
    }

    public void SetOnBlock(bool b)
    {
        onBlock = b;
    }

    public override void FinishParry()
    {
        base.FinishParry();
        parryParticles.Stop();
    }
}
