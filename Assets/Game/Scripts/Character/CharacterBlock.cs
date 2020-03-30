using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterBlock
{
    public bool onBlock;
    public bool onParry;
    float _currentTime;
    float _timer;
    bool canupdate;

    Action beginparry;
    Action endparry;

    CharacterAnimator anim;

    public CharacterBlock(float timer, Action _BeginParry, Action _EndParry, CharacterAnimator _anim)
    {
        _timer = timer;
        _currentTime = timer;
        beginparry = _BeginParry;
        endparry = _EndParry;
        anim = _anim;
    }


    public void OnBlockDown() { onBlock = true; anim.Block(true); }
    public void OnBlockUp() { onBlock = false; anim.Block(false); }

    public void Parry()
    {
        if (!canupdate)
        {
            beginparry.Invoke();
            onParry = true;
            canupdate = true;
        }
    }

    public void OnUpdate()
    {
        if (canupdate)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= _timer/3)
            {
                onParry = false;
                if (_currentTime <= 0)
                {
                    endparry.Invoke();
                    canupdate = false;
                    _currentTime = _timer;
                    
                }
            }
        }
    }
}
