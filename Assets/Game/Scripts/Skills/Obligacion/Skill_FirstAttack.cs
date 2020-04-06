using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skill_FirstAttack : SkillBase
{
    List<PetrifyComponent> petrifyComponents = new List<PetrifyComponent>();
    public float petrifyRange = 100;
    private CharacterHead _hero;
    [SerializeField]
    bool _firstAttack;

    protected override void OnBeginSkill()
    {

        if (_firstAttack)
        {
            petrifyComponents = new List<PetrifyComponent>();
            petrifyComponents = FindObjectsOfType<PetrifyComponent>().ToList();

            foreach (var item in petrifyComponents)
            {
                if (item != null)
                {
                    item.Configure(ReceivePetrifyOnDeathMinion);
                    item.OnBegin();
                }
            }
            //if (_hero == null)
            //    _hero = FindObjectOfType<CharacterHead>();
            //_hero.AddListenerToFirstAttack(AddFirstAttack);
            FirstAttackReady(false);
        }
        
    }
    protected override void OnEndSkill()
    {
        foreach (var item in petrifyComponents)
        {
            if (item != null) item.OnEnd();
        }
        FirstAttackReady(false);
        // _hero.RemoveListenerToFirstattack(AddFirstAttack);

    }

    protected override void OnUpdateSkill()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            FirstAttackReady(true);
        }

    }

    public void ReceivePetrifyOnDeathMinion(Vector3 pos, PetrifyComponent p)
    {
        var listOfEntities = Physics.OverlapSphere(pos, petrifyRange);

        petrifyComponents.Remove(p);

        foreach (var item in listOfEntities)
        {
            EnemyBase myEnemy = item.GetComponent<EnemyBase>();
            if (myEnemy)
            {
                myEnemy.OnPetrified();
            }
        }
    }

    private void AddFirstAttack()
    {
        _hero.charAttack.FirstAttackReady(true);
        _hero.charAttack.PasiveFirstAttackReady(true);
    }
    private void RemoveFirstAttack()
    {
        _hero.charAttack.FirstAttackReady(false);
        _hero.charAttack.PasiveFirstAttackReady(false);
    }

    public void FirstAttackReady(bool ready)
    {
        _firstAttack = ready;
    }
}
