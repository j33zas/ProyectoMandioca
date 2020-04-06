using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MedusaParry_skill : SkillBase
{
    //Para poder usar la duracion aca, se tiene que poder decidir cuanto tiempo el OnPetrified State va a durar
    [SerializeField] private float duracion;
    
    List<DummyEnemy> _enemies = new List<DummyEnemy>();
    protected override void OnBeginSkill()
    {
        _enemies = FindObjectsOfType<DummyEnemy>().ToList();

        foreach (DummyEnemy en in _enemies)
        {
            en.OnParried += en.OnPetrified;
        }
    }

    protected override void OnEndSkill()
    {
        foreach (DummyEnemy en in _enemies)
        {
            en.OnParried -= en.OnPetrified;
        }
    }

    protected override void OnUpdateSkill()
    {
 
    }
}
