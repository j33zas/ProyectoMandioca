using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCW : SkillBase
{
    [SerializeField]
    float cdToExplosion;
    [SerializeField]
    float radiousExp;
    [SerializeField]
    int damageExp;
    float timer;

    [SerializeField]
    CharacterHead head;
    [SerializeField]
    ParticleSystem explosionParticles;

    private void Awake()
    {
        explosionParticles.transform.position = head.transform.position;
        explosionParticles.transform.SetParent(head.transform);
    }

    protected override void OnBeginSkill()
    {
        if (timer == 0)
            Explosion();
    }

    protected override void OnEndSkill()
    {
        timer = 0;
        explosionParticles.Stop();
        explosionParticles.gameObject.SetActive(false);
    }

    void Explosion()
    {
        explosionParticles.gameObject.SetActive(true);
        explosionParticles.Play();

        var radiousToExplosion = Physics.OverlapSphere(head.transform.position, radiousExp);

        foreach (var item in radiousToExplosion)
        {
            if (item.GetComponent<EnemyBase>())
            {
                item.GetComponent<EnemyBase>().TakeDamage(damageExp, (item.transform.position - head.transform.position).normalized);
            }
        }
    }

    protected override void OnUpdateSkill()
    {
        timer += Time.deltaTime;

        if (timer >= cdToExplosion)
        {
            EndSkill();
        }
    }
}
