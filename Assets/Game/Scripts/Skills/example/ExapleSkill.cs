using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExapleSkill : SkillBase
{
    public ParticleSystem explosion;
    public ParticleSystem trail;

    protected override void OnBeginSkill()
    {
        trail.Play();
    }

    protected override void OnEndSkill()
    {
        trail.Stop();
    }

    float time;
    protected override void OnUpdateSkill()
    {
        if (time < 1) time = time + 1 * Time.deltaTime;
        else { time = 0; explosion.Play(); }
    }

   
}
