namespace Hybel.DamageSystem
{
    /// <summary>
    /// Interface for any type of damage where some resistances could have been applied or a maximum amount of damage has been exceeded.
    /// </summary>
    public interface IReducedDamage : IDamage
    {
        /// <summary>
        /// Amount of damage that was reduced by resistances.
        /// <para>If the value is negative this means that the resistances were treated like vulnerabilities instead.</para>
        /// </summary>
        public float AmountReduced { get; }
    }
}
