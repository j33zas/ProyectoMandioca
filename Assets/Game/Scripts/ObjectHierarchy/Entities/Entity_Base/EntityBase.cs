using UnityEngine;
public abstract class EntityBase : PlayObject 
{
    [Header("EntitiyBase")]
    public side_type side_Type;
    
    public abstract Attack_Result TakeDamage(int dmg, Vector3 attackDir);
}
