using System.Collections.Generic;

namespace Hybel.DamageSystem
{
    public interface IDamagePool<TDamage> : IEnumerable<TDamage>
        where TDamage : IDamage
    {
        public List<TDamage> Damages { get; }

        public float TotalAmount
        {
            get
            {
                float total = 0;

                foreach (TDamage damage in this)
                    total += damage.Amount;

                return total;
            }
        }
    }

    public interface IDamagePool : IDamagePool<IDamage> { }
}
