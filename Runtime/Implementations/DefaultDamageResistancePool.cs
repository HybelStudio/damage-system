using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Hybel.DamageSystem
{
    /// <summary>
    /// Default implementation of <see cref="IDamageResistancePool"/>.
    /// </summary>
    public readonly struct DefaultDamageResistancePool : IDamageResistancePool
    {
        private readonly List<IDamageResistance> _resistances;

        public DefaultDamageResistancePool(IDamageResistance[] resistances) => _resistances = resistances.ToList();

        public IReadOnlyList<IDamageResistance> Resistances => _resistances;

        public IEnumerator<IDamageResistance> GetEnumerator() => _resistances.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}