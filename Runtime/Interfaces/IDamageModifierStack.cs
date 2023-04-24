using System.Collections.Generic;

namespace Hybel.DamageSystem
{
    public interface IDamageModifierStack
    {
        public List<IDamageModifier> Modifiers { get; }
        public IComparer<IDamageModifier> Comparer { get; }

        public void Sort();

        public class ByOrderComparer : IComparer<IDamageModifier>
        {
            public int Compare(IDamageModifier x, IDamageModifier y)
            {
                if (x.Order > y.Order)
                    return 1;

                if (x.Order < y.Order)
                    return -1;

                return 0;
            }
        }
    }
}
