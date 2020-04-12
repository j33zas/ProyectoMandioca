using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_LightBounce : SkillActivas
{
    [SerializeField] private int damage;
    [SerializeField] private Damagetype dmgType;
    [SerializeField] private float range;
    
    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;
    
    private CharacterHead _hero;
    private EntityBlock blocker;

    [SerializeField] private LineRenderer _lineRenderer;
    
    protected override void OnExecute() { }

    protected override void OnBeginSkill()
    {
        _hero = Main.instance.GetChar();
        blocker = _hero.GetCharBlock();
    }
    protected override void OnEndSkill() { }

    protected override void OnUpdateSkill()
    {
        if (blocker.onBlock)
        {
            ShootLaserFromTargetPosition(_hero.transform.position + Vector3.up * 1, _hero.GetCharMove().GetLookDirection(), range);
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }

    }
    
    void ShootLaserFromTargetPosition( Vector3 targetPosition, Vector3 direction, float length )
    {
        Ray ray = new Ray( targetPosition, direction );
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + ( length * direction );
 
        if( Physics.Raycast( ray, out raycastHit, length ) ) 
        {
            if (raycastHit.collider.gameObject.GetComponent<EnemyBase>())
            {
                raycastHit.collider.gameObject.GetComponent<EnemyBase>().TakeDamage(damage, targetPosition, dmgType);
            }else
                endPosition = raycastHit.point;
        }
 
        _lineRenderer.SetPosition( 0, targetPosition );
        _lineRenderer.SetPosition( 1, endPosition );
    }
    
}
