using UnityEngine;
public abstract class EntityBase : PlayObject 
{
    [Header("EntitiyBase")]
    public side_type side_Type;
    public abstract Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype);
    public virtual Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase owner_entity) { return TakeDamage(dmg, attack_pos, damagetype); }
}
