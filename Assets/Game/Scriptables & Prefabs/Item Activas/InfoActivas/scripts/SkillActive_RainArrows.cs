using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class SkillActive_RainArrows : SkillActivas
{
    [SerializeField] private float duration;
    [SerializeField] private float dmgTotal;
    [SerializeField] private int ticksAmount;
    private float dmgPerTick;
    [SerializeField] private float range;
    private float tickCount;
    private float skillCount;

    [SerializeField] private ParticleSystem arrowRain_ps;
    private Vector3 anchorPos;

    private CharacterHead _hero;
    private bool isActive = false;
    protected override void OnOneShotExecute()
    {
        dmgPerTick =   dmgTotal / (duration / ticksAmount);
        SetPositionOfRainEffect();
        SetRainFeedBackParticlesPosition();
        arrowRain_ps.Play();
        isActive = true;
        FindAndDamage();
    }

    protected override void OnBeginSkill()
    {

        _hero = Main.instance.GetChar();

    }
    protected override void OnEndSkill() { }

    protected override void OnUpdateSkill()
    {
        if (!isActive)
            return;


        skillCount += Time.deltaTime;
        tickCount += Time.deltaTime;

        if (tickCount < ticksAmount)
            return;

        FindAndDamage();
        tickCount = 0;

        if (skillCount >= duration)
        {
            skillCount = 0;
            isActive = false;
            arrowRain_ps.Stop();
        }
    }

    private void SetRainFeedBackParticlesPosition()
    {
        arrowRain_ps.transform.localPosition = anchorPos + Vector3.up * 3;
    }

    private void SetPositionOfRainEffect()
    {
        anchorPos = _hero.transform.position;
    }

    private void FindAndDamage()
    {
        List<EnemyBase> enemigosAfectados = Extensions.FindInRadius<EnemyBase>(_hero.transform, range);

        DoDamageTo(enemigosAfectados);
    }
    

    private void DoDamageTo<T>(List<T> enemiesAffected) where T : EnemyBase
    {
        foreach (T en in enemiesAffected)
        {
            en.TakeDamage(Mathf.RoundToInt(dmgPerTick), Vector3.up, Damagetype.normal, _hero);
        }
    }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateUse() { }
}
