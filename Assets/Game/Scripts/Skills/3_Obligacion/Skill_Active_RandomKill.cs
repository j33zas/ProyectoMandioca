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
        Debug.Log("Instakill");
        _myEnemys = Main.instance.GetNoOptimizedListEnemies();
        if (_myEnemys.Count > 0)
        {
            int index = Random.Range(0, _myEnemys.Count);
            _myEnemys[index].TakeDamage(200, transform.position, Damagetype.normal, Main.instance.GetChar());
            _player.TakeDamage(_playerDMGReceive, _player.transform.position, Damagetype.normal);
            _player.transform.position = _myEnemys[index].transform.position;
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
