using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skill_FirstAttack : SkillBase
{
    List<PetrifyComponent> petrifyComponents = new List<PetrifyComponent>();
    public float petrifyRange = 100;
    private CharacterHead _hero;

    CharacterAttack charattack;

    EntityBase entity;

    private void Start()
    {
        charattack = Main.instance.GetChar().GetCharacterAttack();
    }

    protected override void OnBeginSkill()
    {
        Main.instance.GetChar().Attack += ReceivePetrifyOnDeathMinion;
        charattack.ActiveFirstAttack();
        charattack.AddCAllback_ReceiveEntity(RecieveEntity);
    }
    protected override void OnEndSkill()
    {
        foreach (var item in petrifyComponents)
        {
            if (item != null) item.OnEnd();
        }

        Main.instance.GetChar().Attack -= ReceivePetrifyOnDeathMinion;
        charattack.DeactiveFirstAttack();
        charattack.RemoveCAllback_ReceiveEntity(RecieveEntity);

    }

    protected override void OnUpdateSkill()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            charattack.ActiveFirstAttack();
        }
    }

    public void ReceivePetrifyOnDeathMinion()
    {
        //var listOfEntities = Physics.OverlapSphere(pos, petrifyRange);
        if (charattack.IsFirstAttack())
        {
            foreach (var item in Main.instance.GetEnemies())
            {
                EnemyBase myEnemy = item.GetComponent<EnemyBase>();
                if (myEnemy)
                {
                    myEnemy.OnPetrified();
                }
            }
            charattack.DeactiveFirstAttack();
        }
    }

    public void RecieveEntity(EntityBase _entity)
    {
        entity = _entity;
    }

    //private void AddFirstAttack()
    //{
    //    //_hero.charAttack.FirstAttackReady(true);
    //    //_hero.charAttack.PasiveFirstAttackReady(true);
    //}
    //private void RemoveFirstAttack()
    //{
    //    //_hero.charAttack.FirstAttackReady(false);
    //    //_hero.charAttack.PasiveFirstAttackReady(false);
    //}
}
