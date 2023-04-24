namespace Hybel.DamageSystem
{
    /// <summary>
    /// Interface for any type of damage with an optional damage type.
    /// </summary>
    public interface IDamage
    {
        public float Amount { get; }

        public IDamageType DamageType { get; }
    }

    public static class DamageExtensions
    {
        public static void Deconstruct(this IDamage damage, out float amount, out IDamageType damageType)
        {
            amount = damage.Amount;
            damageType = damage.DamageType;
        }
    }
}