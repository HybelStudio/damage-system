using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Hybel.DamageSystem
{
    public readonly struct DamagePool : IDamagePool
    {
        private readonly Option<List<IDamage>> _damages;

        public DamagePool(List<IDamage> damages) => _damages = damages;

        public List<IDamage> Damages => _damages.HasValue ? _damages.Unwrap() : null;

        public IEnumerator<IDamage> GetEnumerator()
        {
            if (Damages != null)
                return Damages.GetEnumerator();

            return new List<IDamage>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static IDamagePool Create(params IDamage[] damages) => new DamagePool(damages.ToList());
    }
}
