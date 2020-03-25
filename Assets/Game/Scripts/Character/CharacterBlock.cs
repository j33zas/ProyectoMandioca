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
    bool enableToParry=true;

    public CharacterBlock(float timer)
    {
        _timer = timer;
        _currentTime = timer;
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
        if (enableToParry)
        {
            onParry = true;
            enableToParry = false;
            Debug.Log("Parry");
        }
       
    }

    public void OnUpdate()
    {
        if (!enableToParry)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= _timer/3)
            {
                onParry = false;
                Debug.Log("Parry false");
                if (_currentTime <= 0)
                {
                    enableToParry = true;
                    _currentTime = _timer;
                }

            }
        }
       
    }
}
