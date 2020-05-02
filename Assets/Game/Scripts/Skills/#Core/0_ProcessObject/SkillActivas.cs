using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SkillActivas : SkillBase
{
    Action<SkillInfo, float> CallbackCooldown = delegate { };

    [Header("Cooldown Settings")]
    public float cooldown;
    float time_cooldown;
    bool begincooldown;

    [Header("Use in Time Settings")]
    public bool one_time_use = true;
    public float useTime = 5f;
    float timer_use = 0;
    bool beginUse;
    Func<bool> predicate;
    bool usePredicate;

    public void SetCallbackCooldown(Action<SkillInfo, float> callback)
    {
        CallbackCooldown = callback;
    }
    protected void SetPredicate(Func<bool> pred)
    {
        predicate = pred;
        usePredicate = true;
    }

    public override void BeginSkill()
    {
        begincooldown = true;
        ui_skill.SetImages(skillinfo.img_avaliable, skillinfo.img_actived);
        ui_skill.Cooldown_ConfigureTime(cooldown);
        base.BeginSkill();
    }
    public override void EndSkill()
    {
        base.EndSkill();
    }

    public void Execute()
    {
        if (usePredicate)
            if (!predicate()) return;

        if (!begincooldown)
        {
            begincooldown = true;
            time_cooldown = 0;

            if (one_time_use)
            {
                OnOneShotExecute();
            }
            else
            {
                timer_use = 0;
                OnStartUse();
                beginUse = true;
            }
        }
    }

    protected abstract void OnStartUse();
    protected abstract void OnStopUse();
    protected abstract void OnUpdateUse();

    internal override void absUpdate()
    {
        base.absUpdate();
        if (beginUse)
        {
            if (timer_use < useTime)
            {
                timer_use = timer_use + 1 * Time.deltaTime;
                OnUpdateUse();
            }
            else
            {
                timer_use = 0;
                beginUse = false;
                OnStopUse();
            }
        }
        
    }

    internal override void cooldownUpdate()
    {
        base.cooldownUpdate();
        if (begincooldown)
        {
            if (time_cooldown < cooldown)
            {
                time_cooldown = time_cooldown + 1 * Time.deltaTime;
                CallbackCooldown(skillinfo, time_cooldown);
                //ui_skill.Cooldown_SetValueTime(time_cooldown);
            }
            else
            {
                time_cooldown = 0;
                begincooldown = false;
            }
        }
    }

    protected abstract void OnOneShotExecute();
}
