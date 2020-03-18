using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatBase
{
    FrontendStatBase uilife;

    //variables
    public enum EV_LIFE { LOSE_LIFE, GAIN_LIFE, ON_START, ON_DEATH }

    public event Action loselife;
    public event Action gainlife;
    //public event Action start;
    public event Action death;

    int health;
    int maxHealth;


    public int MaxHealth
    {
        get { return maxHealth; }
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (value > 0)
            {
                if (value > maxHealth) 
                {
                    health = maxHealth;
                    uilife.OnLifeChange(maxHealth, maxHealth);
                }
                else
                {
                    if (value < health)
                    {
                        gainlife();
                    }
                    else if (value > health)
                    {
                        loselife();
                    }
                    uilife.OnLifeChange(value, maxHealth);
                    health = value;
                }
            }
            else
            {
                death();
                health = 0;
                uilife.OnLifeChange(0, maxHealth);
            }
        }
    }

    //Constructor

    public StatBase(int maxHealth, FrontendStatBase uilife, int initial_Life = -1)
    {
        this.maxHealth = maxHealth;
        health = initial_Life == -1 ? this.maxHealth : initial_Life;
        this.uilife = uilife;
        uilife.OnLifeChange(health, maxHealth);
    }

    public void ResetLife()
    {
        Health = maxHealth;
    }

    public void IncreaseLife(int val)
    {
        maxHealth += val;
        Health = maxHealth;
    }

    public void SetLife(int val)
    {
        maxHealth = val;
        Health = maxHealth;
    }

    public void AddEventListener_LoseLife(Action listener) { loselife += listener; }
    public void AddEventListener_GainLife(Action listener) { gainlife += listener; }
    public void AddEventListener_Death(Action listener) { death += listener; }



}
