using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_DamageInRoom : SkillActivas
{
    [SerializeField] private int damagePower;
    [SerializeField] private ParticleSystem feedback;
    [SerializeField] private Damagetype dmgType;

    protected override void OnOneShotExecute()
    {
        List<EnemyBase> enemies = Main.instance.GetEnemies();

        feedback.transform.position = Main.instance.GetChar().transform.position;
        feedback.Play();
        
        foreach (EnemyBase enemy in enemies)
        {
            enemy.TakeDamage(damagePower, Vector3.up, dmgType, Main.instance.GetChar());
        }
    }

    protected override void OnBeginSkill() { }
    protected override void OnEndSkill() { }
    protected override void OnUpdateSkill() { }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateUse() { }
}
