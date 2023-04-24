using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Hybel.DamageSystem
{
    public readonly struct ReducedDamagePool : IReducedDamagePool
    {
        private readonly Option<List<IReducedDamage>> _damages;

        public ReducedDamagePool(List<IReducedDamage> damages) => _damages = damages;

        public List<IReducedDamage> Damages => _damages.DangerousValue;

        public IEnumerator<IReducedDamage> GetEnumerator()
        {
            if (Damages != null)
                return Damages.GetEnumerator();

            return new List<IReducedDamage>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static IReducedDamagePool Create(IDamagePool sourceDamagePool, IEnumerable<IDamageResistance> damageResistances) =>
            Create(sourceDamagePool, damageResistances.ToArray());

        public static IReducedDamagePool Create(IDamagePool sourceDamagePool, IDamageResistancePool damageResistances) =>
            damageResistances == null ? Create(sourceDamagePool) : Create(sourceDamagePool, damageResistances.Resistances);

        public static IReducedDamagePool Create(IDamagePool sourceDamagePool, params IDamageResistance[] damageResistances)
        {
            List<IReducedDamage> reducedDamages = new();

            foreach (IDamage damage in sourceDamagePool)
            {
                IReducedDamage reducedDamage = ReducedDamage.Create(damage, damageResistances);
                reducedDamages.Add(reducedDamage);
            }

            return new ReducedDamagePool(reducedDamages);
        }

        internal static IReducedDamagePool Internal_Create(IDamagePool sourceDamagePool, Option<IDamageResistancePool> damageResistancePool)
        {
            List<IReducedDamage> reducedDamages = new();

            foreach (IDamage damage in sourceDamagePool)
            {
                IReducedDamage reducedDamage = ReducedDamage.Create(damage, damageResistancePool.DangerousValue);
                reducedDamages.Add(reducedDamage);
            }

            return new ReducedDamagePool(reducedDamages);
        }
    }
}
