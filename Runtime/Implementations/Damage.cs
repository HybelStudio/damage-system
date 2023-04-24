using System;
using UnityEngine;

namespace Hybel.DamageSystem
{
    public readonly struct Damage : IDamage
    {
        private readonly float _amount;
        private readonly Option<IDamageType> _damageType;

        public Damage(float amount) : this(amount, null) { }
        public Damage(float amount, IDamageType damageType)
        {
            _amount = Mathf.Max(amount, 0f);
            _damageType = damageType.Some();
        }

        public float Amount => _amount;

        public IDamageType DamageType => _damageType.DangerousValue;

        public static IDamage Create(float baseDamage, params IDamageModifier[] damageModifiers) =>
            Create((damageValue, type) => new Damage(damageValue, type), baseDamage, null, new DefaultDamageModifierStack(damageModifiers));

        public static IDamage Create(float baseDamage, IDamageModifierStack damageModifierStack) =>
            Create((damageValue, type) => new Damage(damageValue, type), baseDamage, null, damageModifierStack);

        public static IDamage Create(float baseDamage, IDamageType damageType, params IDamageModifier[] damageModifiers) =>
            Create((damageValue, type) => new Damage(damageValue, type), baseDamage, damageType, new DefaultDamageModifierStack(damageModifiers));

        public static IDamage Create(float baseDamage, IDamageType damageType, IDamageModifierStack damageModifierStack) =>
            Create((damageValue, type) => new Damage(damageValue, type), baseDamage, damageType, damageModifierStack);

        public static IDamage Create(Func<float, IDamageType, IDamage> damageCreateFunc, float baseDamage, params IDamageModifier[] damageModifiers) => 
            Create(damageCreateFunc, baseDamage, null, new DefaultDamageModifierStack(damageModifiers));

        public static IDamage Create(Func<float, IDamageType, IDamage> damageCreateFunc, float baseDamage, IDamageModifierStack damageModifierStack) =>
            Create(damageCreateFunc, baseDamage, null, damageModifierStack);

        public static IDamage Create(Func<float, IDamageType, IDamage> damageCreateFunc, float baseDamage, IDamageType damageType, params IDamageModifier[] damageModifiers) =>
            Create(damageCreateFunc, baseDamage, damageType, new DefaultDamageModifierStack(damageModifiers));

        public static IDamage Create(Func<float, IDamageType, IDamage> damageCreateFunc, float baseDamage, IDamageType damageType, IDamageModifierStack damageModifierStack)
        {
            damageModifierStack.Sort();
            IDamageModifier[] damageModifiers = damageModifierStack.Modifiers.ToArray();
            float totalDamage = baseDamage;
            float sumPercentAdditive = 1f;

            for (int i = 0; i < damageModifiers.Length; i++)
            {
                var damageModifier = damageModifiers[i];

                switch (damageModifier.ModifierType)
                {
                    case IDamageModifier.Type.Additive:
                        totalDamage = HandleAdditive(totalDamage, damageModifier);
                        break;

                    case IDamageModifier.Type.PercentAdditive:
                        totalDamage = HandlePercentAdditive(totalDamage, damageModifier, i, damageModifiers, ref sumPercentAdditive);
                        break;

                    case IDamageModifier.Type.PercentMultiplicative:
                        totalDamage = HandlePercentMultiplicative(totalDamage, damageModifier);
                        break;
                }
            }

            return damageCreateFunc(totalDamage, damageType);

            static float HandleAdditive(float totalDamage, IDamageModifier damageModifier)
            {
                totalDamage += damageModifier.Modifier;
                return totalDamage;
            }

            static float HandlePercentAdditive(float totalDamage, IDamageModifier damageModifier, int i, IDamageModifier[] damageModifiers, ref float sumPercentAdditive)
            {
                var modifier = damageModifier.Modifier;

                sumPercentAdditive += modifier; // Accumulate Percent Additive

                int nextIndex = i + 1;
                bool isOnLastElementInArray = nextIndex == damageModifiers.Length;

                if (isOnLastElementInArray || damageModifiers[nextIndex].ModifierType != IDamageModifier.Type.PercentAdditive)
                {
                    totalDamage *= sumPercentAdditive;
                    sumPercentAdditive = 1f;
                }

                return totalDamage;
            }

            static float HandlePercentMultiplicative(float totalDamage, IDamageModifier damageModifier)
            {
                totalDamage *= damageModifier.Modifier;
                return totalDamage;
            }
        }

        public static implicit operator Damage(float damageValue) => new Damage(damageValue);
    }
}
