using System.Linq;

namespace Hybel.DamageSystem
{
    public interface IDamageModifier
    {
        /// <summary>
        /// The value which is used when modifying a damage value.
        /// <para>A modifier with type <see cref="Type.Additive"/> will add <see cref="Modifier"/> to the current damage value immediatly.</para>
        /// <para>A modifier with type <see cref="Type.PercentAdditive"/> will sum all <see cref="Modifier"/> first, then multiply the sum to the current damage value.</para>
        /// <para>A modifier with type <see cref="Type.PercentMultiplicative"/> will multiply <see cref="Modifier"/> to the current damage value immediatly.</para>
        /// <para>In this context the 'current damage value' reffered to is the 'so-far' modified damage value given it has been modified. If it hasn't it reffers to the base damage value.</para>
        /// </summary>
        public float Modifier { get; }

        /// <summary>
        /// This determines how the <see cref="Modifier"/> will be added to the damage value when creating it.
        /// <para>A modifier with type <see cref="Type.Additive"/> will add <see cref="Modifier"/> to the current damage value immediatly.</para>
        /// <para>A modifier with type <see cref="Type.PercentAdditive"/> will sum all<see cref= "Modifier" /> first, then multiply the sum to the current damage value.</para>
        /// <para>A modifier with type <see cref="Type.PercentMultiplicative"/> will multiply<see cref="Modifier"/> to the current damage value immediatly.</para>
        /// <para>In this context the 'current damage value' reffered to is the 'so-far' modified damage value given it has been modified. If it hasn't it reffers to the base damage value.</para>
        /// </summary>
        public Type ModifierType { get; }

        /// <summary>
        /// The order in which this modifier will be added to the damage value. The lower the <see cref="Order"/> is the earlier it will happen.
        /// <para>Each <see cref="Type"/> has a default ordering: <see cref="Type.Additive"/> = 100, <see cref="Type.PercentAdditive"/> = 200, <see cref="Type.PercentMultiplicative"/> = 300.</para>
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// 
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Adds the modifier value directly to the damage.
            /// <para>Default Ordering is 100.</para>
            /// </summary>
            Additive = 100,

            /// <summary>
            /// Sums together evey grouped <see cref="PercentAdditive"/> modifier value, then multiplies the sum with the damage.
            /// <para>Default Ordering is 200.</para>
            /// </summary>
            PercentAdditive = 200,

            /// <summary>
            /// Multiplies the modifier value directly to the damage.
            /// <para>Default Ordering is 300.</para>
            /// </summary>
            PercentMultiplicative = 300,
        }
    }

    public static class IDamageModifierExtensions
    {
        /// <summary>
        /// Checks if any of the <paramref name="damageModifiers"/> have modifier types that are equals to any of the <paramref name="modifierTypes"/>.
        /// </summary>
        /// <returns>True if any of the <paramref name="damageModifiers"/> have modifier types that are equals to any of the <paramref name="modifierTypes"/>.</returns>
        public static bool ContainsAnyModifierWithType(this IDamageModifier[] damageModifiers, params IDamageModifier.Type[] modifierTypes)
        {
            return damageModifiers
                .Select(dm => dm.ModifierType)
                .Any(mt1 => modifierTypes
                    .Any(mt2 => mt1 == mt2));
        }
    }
}