namespace Hybel.DamageSystem
{
    public interface IHasResistances
    {
        public IDamageResistancePool DamageResistances { get; }
    }
}