using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_SomeHeal : SkillActivas
{

    [SerializeField] private int healAmount;
    [SerializeField] private ParticleSystem healFeedback;

    private CharacterHead _hero;
    private LifeSystem _lifeSystem;

    protected override void OnExecute()
    {
        _lifeSystem.AddHealth(healAmount);
        healFeedback.Play();
    }

    protected override void OnBeginSkill()
    {
        _hero = Main.instance.GetChar();
        _lifeSystem = _hero.GetComponent<LifeSystem>();
    }
    protected override void OnEndSkill() { }

    protected override void OnUpdateSkill()
    {
        if (healFeedback.isPlaying)
            healFeedback.transform.position = _hero.transform.position;
    }
}
