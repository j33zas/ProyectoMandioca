using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterBlock
{
    public bool onBlock;

    public CharacterBlock()
    {

    }


    public void OnBlockDown()
    {
        onBlock = true;
    }
    public void OnBlockUp()
    {
        onBlock = false;
    }

    public void Parry()
    {
        Debug.Log("Parry");
    }
}
