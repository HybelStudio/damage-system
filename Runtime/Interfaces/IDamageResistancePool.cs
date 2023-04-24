using System.Collections.Generic;

namespace Hybel.DamageSystem
{
    /// <summary>
    /// A list of resistances each with their own modifier and damage type.
    /// </summary>
    public interface IDamageResistancePool : IEnumerable<IDamageResistance>
    {
        public IReadOnlyList<IDamageResistance> Resistances { get; }
    }
}