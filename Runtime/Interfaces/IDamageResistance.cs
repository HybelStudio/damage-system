using System.Collections.Generic;
using System.Linq;

namespace Hybel.DamageSystem
{
    /// <summary>
    /// A single resistance against a single type of damage.
    /// </summary>
    public interface IDamageResistance
    {
        public IDamageModifier Modifier { get; }
        public List<IDamageType> DamageTypesToResist { get; }
    }

    public static class IDamageResistancePoolExtensions
    {
        /// <summary>
        /// Converts a simple list of damage resistances to a structure where each resistance is grouped by damage type and modifiers are aggregated together based on what damage type they were modifying.
        /// </summary>
        internal static IDamageResistanceGroupPool GroupModifiersByDamageType(this IDamageResistancePool damageResistancePool)
        {
            var damageResistanceGroups = new List<IDamageResistanceGroup>();

            // Create a new structure where the list of damagetypes each have their own modifier
            List<(Option<IDamageType> DamageType, IDamageModifier Modifier)> damageTypeAndDamageModifierList = new List<(Option<IDamageType>, IDamageModifier)>();

            foreach (var resistance in damageResistancePool.Resistances)
            {
                if (resistance.DamageTypesToResist != null)
                {
                    foreach (var damageType in resistance.DamageTypesToResist)
                        damageTypeAndDamageModifierList.Add((damageType.Some(), resistance.Modifier));
                }
                else
                {
                    damageTypeAndDamageModifierList.Add((Option<IDamageType>.Nothing, resistance.Modifier));
                }
            }
            
            // Group all resistances by the same damage type.
            var groups = damageTypeAndDamageModifierList.GroupBy(damageResistance => damageResistance.DamageType);

            foreach (var group in groups)
            {
                Option<IDamageType> damageType = group.Key;
                List<IDamageModifier> modifiers = new List<IDamageModifier>();

                // Get each modifier from the resistances.
                foreach (var element in group)
                    modifiers.Add(element.Modifier);

                // Create an IDamageModifierStack with the modifiers.
                var damageModifierStack = new DefaultDamageModifierStack(modifiers);

                // Create an IDamageResistanceGroup using the modifier stack and the damage type of for that stack.
                var damageResistanceGroup = new DefaultDamageResistanceGroup(damageModifierStack, damageType);

                // Add it to damageResistanceGroups.
                damageResistanceGroups.Add(damageResistanceGroup);
            }

            // Create and return the damage resistance group pool.
            return new DefaultDamageResistanceGroupPool(damageResistanceGroups);
        }
    }
}