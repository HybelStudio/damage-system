using System.Collections.Generic;

namespace Hybel.DamageSystem
{
    /// <summary>
    /// A list of resistance groups where each group should have a unique damage type.
    /// </summary>
    internal interface IDamageResistanceGroupPool : IEnumerable<IDamageResistanceGroup>
    {
        public List<IDamageResistanceGroup> DamageResistanceGroups { get; }
    }
}