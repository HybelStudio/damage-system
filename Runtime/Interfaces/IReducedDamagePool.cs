namespace Hybel.DamageSystem
{
    public interface IReducedDamagePool : IDamagePool<IReducedDamage>
    {
        public float TotalAmountReduced
        {
            get
            {
                float totalReduced = 0;

                foreach (IReducedDamage damage in this)
                    totalReduced += damage.AmountReduced;

                return totalReduced;
            }
        }
    }
}
