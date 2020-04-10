﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HalfLife : SkillBase
{
    CharacterHead character;

    private void Start()
    {
        character = Main.instance.GetChar();
    }
    protected override void OnBeginSkill()
    {
        var enemysInRoom = Main.instance.GetRoom().myenemies();
        for (int i = 0; i < enemysInRoom.Count; i++)
        {
            enemysInRoom[i].HalfLife();

        }
    }

    protected override void OnEndSkill()
    {
        var enemysInRoom = Main.instance.GetRoom().myenemies();
        for (int i = 0; i < enemysInRoom.Count; i++)
        {
            enemysInRoom[i].Mortal();

        }
    }

    protected override void OnUpdateSkill()
    {

    }

    
}