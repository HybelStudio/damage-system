using System.Collections;
using System.Collections.Generic;

namespace Hybel.DamageSystem.Tests
{
    internal readonly struct MockDamageResistance : IDamageResistance
    {
        public IDamageModifier Modifier { get; }
        List<IDamageType> IDamageResistance.DamageTypesToResist { get; }
    }

    internal readonly struct MockDamageResistancePool : IDamageResistancePool
    {
        public IReadOnlyList<IDamageResistance> Resistances { get; }

        public IEnumerator<IDamageResistance> GetEnumerator() => Resistances.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
