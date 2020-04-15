using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class SkillActive_FreezeRange : SkillActivas
{
    [SerializeField] private float range;
    [SerializeField] private int freezeDuration;
    [SerializeField] private ParticleSystem freeze_ps_pf;
    
    List<ParticleSystem> ps = new List<ParticleSystem>();

    private CharacterHead _hero;

    protected override void OnOneShotExecute()
    {
        List<EnemyBase> enemies = Extensions.FindInRadius<EnemyBase>(_hero.transform, range);

        foreach (EnemyBase enemy in enemies)
        {
            var newPs = Instantiate(freeze_ps_pf, enemy.transform);
            Destroy(newPs, freezeDuration); 
            enemy.OnFreeze();
        }
    }

    protected override void OnBeginSkill()
    {
        _hero = Main.instance.GetChar();
    }
    protected override void OnEndSkill() { }
    protected override void OnUpdateSkill() { }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateUse() { }
}
