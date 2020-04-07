using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGuiltPassive : SkillBase
{
    [SerializeField]
    float buffDamage;
    [SerializeField]
    float timeToBuff;
    float timer;

    CharacterHead head;

    [SerializeField]
    ParticleSystem feedbackParticle;

    bool buffActived;
    private void Start()
    {
        head = Main.instance.GetChar();
        feedbackParticle.transform.position = head.transform.position;
        feedbackParticle.transform.SetParent(head.transform);
    }
    protected override void OnBeginSkill()
    {

    }

    protected override void OnEndSkill()
    {
        
    }

    protected override void OnUpdateSkill()
    {

    }

    void SpawnScream(params object[] param)
    {

    }

    void PetrifyAllEnemies()
    {

    }
}
