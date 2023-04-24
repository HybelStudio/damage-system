namespace Hybel.DamageSystem.Tests
{
    // Mock implementation since an IDamagable is meant to be implemented by the user.
    internal struct MockDamagable : IAdvancedDamagable, IHasResistances
    {
        public float Health { get; set; }
        public IDamageResistancePool DamageResistances { get; set; }

        public IReducedDamage TakeDamage(float damage) => TakeDamage(new Damage(damage));

        public IReducedDamage TakeDamage(IDamage damage)
        {
            IReducedDamage reducedDamage = ReducedDamage.Create(damage, DamageResistances);
            Health -= reducedDamage.Amount;
            return reducedDamage;
        }

        public IReducedDamagePool TakeDamage(IDamagePool damagePool)
        {
            IReducedDamagePool reducedDamagePool = ReducedDamagePool.Internal_Create(damagePool, new Option<IDamageResistancePool>(DamageResistances));

            foreach (IDamage damage in damagePool)
                TakeDamage(damage);

            return reducedDamagePool;
        }
    }
}
