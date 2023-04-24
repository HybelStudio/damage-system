namespace Hybel.DamageSystem.Tests
{
    internal struct MockDamage : IDamage
    {
        public float Amount { get; }
        public IDamageType DamageType { get; }
    }
}