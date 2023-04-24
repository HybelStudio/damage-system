namespace Hybel.DamageSystem
{
    public readonly struct DefaultDamageModifier : IDamageModifier
    {
        private readonly float _modifier;
        private readonly IDamageModifier.Type _modifierType;
        private readonly int _order;

        public DefaultDamageModifier(float modifier, IDamageModifier.Type modifierType) : this(modifier, modifierType, (int)modifierType) { }
        public DefaultDamageModifier(float modifier, IDamageModifier.Type modifierType, int order)
        {
            _modifier = modifier;
            _modifierType = modifierType;
            _order = order;
        }

        public float Modifier => _modifier;
        public IDamageModifier.Type ModifierType => _modifierType;
        public int Order => _order;
    }
}