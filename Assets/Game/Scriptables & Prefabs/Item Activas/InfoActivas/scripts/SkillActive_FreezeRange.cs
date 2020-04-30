using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class SkillActive_FreezeRange : SkillActivas
{
    [SerializeField] private float range;
    [SerializeField] private int freezeDuration;
    [SerializeField] private ParticleSystem freeze_ps_pf;

    [SerializeField] private ParticleSystem freezeNova;
    
    List<ParticleSystem> ps = new List<ParticleSystem>();

    private CharacterHead _hero;

    protected override void OnOneShotExecute()
    {
        freezeNova.transform.position = _hero.transform.position + Vector3.up * .2f;
        var auraMain = freezeNova.main;
        auraMain.duration = freezeDuration;
        auraMain.startSize = range;
        freezeNova.Play();
        
        List<EnemyBase> enemies = Extensions.FindInRadius<EnemyBase>(_hero.transform, range);

        
        foreach (EnemyBase enemy in enemies)
        {
            //var newPs = Instantiate(freeze_ps_pf, enemy.transform);
            //Destroy(newPs, freezeDuration); 
            
            
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
