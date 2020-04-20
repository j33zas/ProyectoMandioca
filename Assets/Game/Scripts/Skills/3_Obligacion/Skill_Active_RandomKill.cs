using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Active_RandomKill : SkillActivas
{
    List<EnemyBase> _myEnemys = new List<EnemyBase>();
    CharacterHead _player;
    [SerializeField] int _playerDMGReceive;
    protected override void OnBeginSkill()
    {
        _player = Main.instance.GetChar();
    }

    protected override void OnEndSkill()
    {
    }

    protected override void OnOneShotExecute()
    {
        _myEnemys = Main.instance.GetRoom().myEnemies;
        if (_myEnemys.Count < 0)
        {
            int index = Random.Range(0, _myEnemys.Count);
            EnemyBase deathEnemy = _myEnemys[index];
            deathEnemy.InstaKill();
            _player.TakeDamage(_playerDMGReceive, _player.transform.position, Damagetype.normal);
        }
    }

    protected override void OnStartUse()
    {
    }

    protected override void OnStopUse()
    {
    }

    protected override void OnUpdateSkill()
    {
    }

    protected override void OnUpdateUse()
    {
    }

   
}
