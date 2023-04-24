namespace Hybel.DamageSystem
{
    /// <summary>
    /// A group of modifiers all modifying damage against the same damage type.
    /// </summary>
    internal interface IDamageResistanceGroup
    {
        public IDamageModifierStack DamageModifierStack { get; }
        public Option<IDamageType> DamageType { get; }
    }

}