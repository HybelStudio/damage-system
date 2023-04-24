namespace Hybel.DamageSystem
{
    /// <summary>
    /// Default implementation of <see cref="IDamageResistanceGroup"/>.
    /// </summary>
    internal readonly struct DefaultDamageResistanceGroup : IDamageResistanceGroup
    {
        private readonly IDamageModifierStack _damageModifierStack;
        private readonly Option<IDamageType> _damageType;

        public DefaultDamageResistanceGroup(IDamageModifierStack damageModifierStack) : this(damageModifierStack, Option<IDamageType>.Nothing) { }
        public DefaultDamageResistanceGroup(IDamageModifierStack damageModifierStack, Option<IDamageType> damageType)
        {
            _damageModifierStack = damageModifierStack;
            _damageType = damageType;
        }

        public IDamageModifierStack DamageModifierStack => _damageModifierStack;
        public Option<IDamageType> DamageType => _damageType;
    }
}