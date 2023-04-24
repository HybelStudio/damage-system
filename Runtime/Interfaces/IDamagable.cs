namespace Hybel.DamageSystem
{
    public interface IDamagable
    {
        public IReducedDamage TakeDamage(float damage);
    }

    public interface IAdvancedDamagable : IDamagable
    {
        public IReducedDamage TakeDamage(IDamage damage);
        public IReducedDamagePool TakeDamage(IDamagePool damagePool);
    }
}