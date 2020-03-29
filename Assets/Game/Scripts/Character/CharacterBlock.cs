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

    public CharacterBlock(float timer, Action _BeginParry, Action _EndParry)
    {
        _timer = timer;
        _currentTime = timer;
        beginparry = _BeginParry;
        endparry = _EndParry;
    }


    public void OnBlockDown()
    {
        onBlock = true;
        Debug.Log("block");
    }
    public void OnBlockUp()
    {
        onBlock = false;
    }

    public void Parry()
    {
        if (!canupdate)
        {
            beginparry.Invoke();
            onParry = true;
            canupdate = true;
            Debug.Log("Parry");
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
                Debug.Log("Parry false");
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
