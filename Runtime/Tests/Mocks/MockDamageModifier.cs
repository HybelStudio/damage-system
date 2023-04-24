namespace Hybel.DamageSystem.Tests
{
    internal struct MockDamageModifier : IDamageModifier
    {
        public float Modifier { get; }
        public IDamageModifier.Type ModifierType { get; }
        public int Order { get; }
    }
}