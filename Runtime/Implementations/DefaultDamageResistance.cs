using System.Collections.Generic;

namespace Hybel.DamageSystem
{
    /// <summary>
    /// Default implementation of <see cref="IDamageResistance"/>.
    /// </summary>
    public readonly struct DefaultDamageResistance : IDamageResistance
    {
        private readonly IDamageModifier _modifier;
        private readonly Option<List<Option<IDamageType>>> _damageTypes;

        public DefaultDamageResistance(IDamageModifier modifier) : this(modifier, damageTypes: null) { }
        public DefaultDamageResistance(IDamageModifier modifier, IDamageType damageType) : this(modifier, new List<IDamageType> { damageType }) { }
        public DefaultDamageResistance(IDamageModifier modifier, List<IDamageType> damageTypes)
        {
            _modifier = modifier;

            if (damageTypes == null)
            {
                _damageTypes = Option<List<Option<IDamageType>>>.Nothing;
            }
            else
            {
                List<Option<IDamageType>> newDamageTypes = new List<Option<IDamageType>>();

                foreach (var damageType in damageTypes)
                    newDamageTypes.Add(damageType.Some());

                _damageTypes = newDamageTypes;
            }
        }

        public IDamageModifier Modifier => _modifier;
        public List<IDamageType> DamageTypesToResist
        {
            get
            {
                if (_damageTypes.TryUnwrap(out var damageTypeOptions))
                {
                    var newList = new List<IDamageType>();

                    foreach (var damageTypeOption in damageTypeOptions)
                        if (damageTypeOption.TryUnwrap(out var damageType))
                            newList.Add(damageType);

                    return newList;
                }

                return null;
            }
        }
    }
}