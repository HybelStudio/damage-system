using System.Collections.Generic;
using System.Linq;

namespace Hybel.DamageSystem
{
    public readonly struct DefaultSortedDamageModifierStack : IDamageModifierStack
    {
        private readonly List<IDamageModifier> _modifiers;
        private readonly IComparer<IDamageModifier> _comparer;

        public DefaultSortedDamageModifierStack(IEnumerable<IDamageModifier> damageModifiers)
        {
            _modifiers = damageModifiers.ToList();
            _comparer = DamageModifierUtils.DefaultComparer;
            Sort();
        }

        public List<IDamageModifier> Modifiers => _modifiers;
        public IComparer<IDamageModifier> Comparer => _comparer;

        public void Sort() => Modifiers.Sort(Comparer);
    }
}
