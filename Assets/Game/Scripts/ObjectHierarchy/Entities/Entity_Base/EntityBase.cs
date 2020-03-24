public abstract class EntityBase : PlayObject 
{
    public enum side_type { neutral, ally, enemy }
    public side_type side_Type;
    public abstract void TakeDamage(int dmg);
}
