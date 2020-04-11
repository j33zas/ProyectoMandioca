using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MedusaParry_skill : SkillBase
{
    //Para poder usar la duracion aca, se tiene que poder decidir cuanto tiempo el OnPetrified State va a durar
    [SerializeField] private float duracion;
    
    List<EnemyBase> _enemies = new List<EnemyBase>();
    protected override void OnBeginSkill()
    {
        _enemies = Main.instance.GetEnemies();

        foreach (var en in _enemies)
        {
            en.OnParried += en.OnPetrified;
        }
    }

    protected override void OnEndSkill()
    {
        foreach (var en in _enemies)
        {
            en.OnParried -= en.OnPetrified;
        }
    }

    protected override void OnUpdateSkill()
    {
 
    }
}
