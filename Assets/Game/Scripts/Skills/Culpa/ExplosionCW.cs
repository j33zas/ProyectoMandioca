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

    CharacterHead head;
    [SerializeField]
    ParticleSystem explosionParticles;

    bool buffActived;
    private void Start()
    {
        head = Main.instance.GetChar();
        explosionParticles.transform.position = head.transform.position;
        explosionParticles.transform.SetParent(head.transform);
    }

    protected override void OnBeginSkill()
    {
        head.ChangeWeaponPassives += IsExplosion;
    }

    void IsExplosion()
    {
        if (!buffActived)
            Explosion();
    }

    protected override void OnEndSkill()
    {
        head.ChangeWeaponPassives -= IsExplosion;
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

        buffActived = true;
    }

    protected override void OnUpdateSkill()
    {
        if (buffActived)
        {
            timer += Time.deltaTime;

            if (timer >= cdToExplosion)
            {
                EndCD();
            }
        }
    }

    void EndCD()
    {
        timer = 0;
        explosionParticles.Stop();
        explosionParticles.gameObject.SetActive(false);
        buffActived = false;
    }
}
