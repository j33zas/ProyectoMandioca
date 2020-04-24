using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireParry_skill : SkillBase
{
    [SerializeField] private Damagetype dmgType;
    [SerializeField] private float doTDuration;
    [SerializeField] private float timePerTick;
    [SerializeField] private int dmgPerTick;

    private List<EnemyBase> _enemies;
    
    protected override void OnBeginSkill()
    {
        _enemies = new List<EnemyBase>();
        _enemies = Main.instance.GetEnemies();

        foreach (var item in _enemies)
        {
            if (item != null)
            {
                item.OnParried += item.OnFire;
            }
        }
    }

    protected override void OnEndSkill()
    {
        foreach (var item in _enemies)
                {
                    if (item != null)
                    {
                        item.OnParried -= item.OnFire;
                    }
                }
    }

    protected override void OnUpdateSkill()
    {
        
    }
    
}
