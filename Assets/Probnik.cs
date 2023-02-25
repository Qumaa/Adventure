public interface IDamageable
{
    void TakeDamage(int damage);
}

public abstract class Enemy : IDamageable
{ 
    public abstract void TakeDamage(int a);
}
public abstract class Vehicle : IDamageable
{
    public abstract void TakeDamage(int a);
}
public abstract class Plant : IDamageable
{ 
    public abstract void TakeDamage(int a); 
}

public class DamageResolver
{
    public void ResolveDamage(IDamageable subject, int damage) => subject.TakeDamage(damage);
}