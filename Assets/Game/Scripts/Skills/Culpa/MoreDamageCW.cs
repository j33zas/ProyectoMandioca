using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreDamageCW : SkillBase
{
    [SerializeField]
    float buffDamage;
    [SerializeField]
    float timeToBuff;
    float timer;

    [SerializeField]
    CharacterHead head;
    [SerializeField]
    ParticleSystem feedbackParticle;
    private void Awake()
    {
        feedbackParticle.transform.position = head.transform.position;
        feedbackParticle.transform.SetParent(head.transform);
    }
    protected override void OnBeginSkill()
    {
        head.ChangeDamage(buffDamage);
        feedbackParticle.gameObject.SetActive(true);
        feedbackParticle.Play();
    }

    protected override void OnEndSkill()
    {
        head.ChangeDamage(-buffDamage);
        timer = 0;
        feedbackParticle.Stop();
        feedbackParticle.gameObject.SetActive(false);
    }

    protected override void OnUpdateSkill()
    {
        timer += Time.deltaTime;

        if (timer >= timeToBuff)
        {
            EndSkill();
        }
    }
}
