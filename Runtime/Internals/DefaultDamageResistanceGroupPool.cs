using System.Collections;
using System.Collections.Generic;

namespace Hybel.DamageSystem
{
    /// <summary>
    /// Default implementation of <see cref="IDamageResistanceGroupPool"/>.
    /// </summary>
    internal struct DefaultDamageResistanceGroupPool : IDamageResistanceGroupPool
    {
        private readonly List<IDamageResistanceGroup> _damageResistanceGroups;

        public DefaultDamageResistanceGroupPool(List<IDamageResistanceGroup> damageResistanceGroups) => _damageResistanceGroups = damageResistanceGroups;

        public List<IDamageResistanceGroup> DamageResistanceGroups => _damageResistanceGroups;

        public IEnumerator<IDamageResistanceGroup> GetEnumerator() => _damageResistanceGroups.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}