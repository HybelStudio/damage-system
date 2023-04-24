namespace Hybel.DamageSystem
{
    public readonly struct ReducedDamage : IReducedDamage
    {
        private readonly float _amount;
        private readonly float _reducedAmount;
        private readonly Option<IDamageType> _damageType;

        public ReducedDamage(float amount, float reducedAmount, IDamageType damageType)
        {
            _amount = amount;
            _reducedAmount = reducedAmount;
            _damageType = damageType.Some();
        }

        public float Amount => _amount;
        public float AmountReduced => _reducedAmount;
        public IDamageType DamageType => _damageType.DangerousValue;

        public static IReducedDamage Create(float damageAmount, params IDamageResistance[] damageResistances) =>
            Create(new Damage(damageAmount), damageResistances);

        public static IReducedDamage Create(float damageAmount, IDamageResistancePool damageResistancePool) =>
            Create(new Damage(damageAmount), damageResistancePool);

        public static IReducedDamage Create(IDamage sourceDamage, float reducedAmount) =>
            new ReducedDamage(sourceDamage.Amount, reducedAmount, sourceDamage.DamageType);

        public static IReducedDamage Create(IDamage sourceDamage, params IDamageResistance[] damageResistances) =>
            Create(sourceDamage, new DefaultDamageResistancePool(damageResistances));

        public static IReducedDamage Create(IDamage sourceDamage, IDamageResistancePool damageResistancePoolOption)
        {
            if (damageResistancePoolOption != null)
            {
                var damageResistanceGroupPool = damageResistancePoolOption.GroupModifiersByDamageType();

                foreach (var damageResistanceGroup in damageResistanceGroupPool)
                {
                    if (sourceDamage.DamageType != damageResistanceGroup.DamageType.DangerousValue)
                        continue;

                    IDamageType sourceDamageType = sourceDamage.DamageType;
                    float sourceDamageAmount = sourceDamage.Amount;
                    var newDamage = Damage.Create(sourceDamageAmount, sourceDamageType, damageResistanceGroup.DamageModifierStack);
                    return Create(newDamage, sourceDamage.Amount - newDamage.Amount);
                }
            }

            return Create(sourceDamage, 0f);
        }
    }
}
