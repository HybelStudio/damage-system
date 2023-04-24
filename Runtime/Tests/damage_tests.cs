using FluentNUnity.Shared;
using NUnit.Framework;
using System.Linq;
using UnityEngine;

using Range = NUnit.Framework.RangeAttribute;

namespace Hybel.DamageSystem.Tests
{
    [Category("Damage")]
    public class damage_tests
    {
        private IDamageModifier.Type[] _modifierOrder = new IDamageModifier.Type[]
        {
            IDamageModifier.Type.Additive,              // 0
            IDamageModifier.Type.Additive,              // 1
            IDamageModifier.Type.PercentAdditive,       // 2
            IDamageModifier.Type.PercentAdditive,       // 3
            IDamageModifier.Type.PercentMultiplicative, // 4
            IDamageModifier.Type.PercentAdditive,       // 5
            IDamageModifier.Type.Additive,              // 6
            IDamageModifier.Type.PercentMultiplicative, // 7
            IDamageModifier.Type.PercentMultiplicative, // 8
            IDamageModifier.Type.PercentAdditive,       // 9
            IDamageModifier.Type.Additive,              // 10
            IDamageModifier.Type.PercentAdditive,       // 11
            IDamageModifier.Type.Additive,              // 12
            IDamageModifier.Type.PercentMultiplicative, // 13
            IDamageModifier.Type.PercentMultiplicative, // 14
        };

        [Test]
        public void create_correct_damage_with_multiple_additive_modifiers([Range(-1f, 1f, 1f)] float damageValue, [Range(-1f, 1f, .5f)] float modifierValue, [Range(0, 3)] int numberOfModifiers)
        {
            // Arrange
            IDamageModifier[] damageModifiers = new IDamageModifier[numberOfModifiers];

            for (int i = 0; i < numberOfModifiers; i++)
                damageModifiers[i] = new DefaultDamageModifier(modifierValue, IDamageModifier.Type.Additive);

            // Act
            IDamage damage = Damage.Create((d, t) => new Damage(d, t), damageValue, damageModifiers);

            // Assert
            var modifierValueArray = damageModifiers.Select(mod => mod.Modifier).ToArray();
            float expectedTotalDamage = Mathf.Max(damageValue + Sum(modifierValueArray), 0f);
            damage.Amount.Should().Be(expectedTotalDamage);
        }

        [Test]
        public void create_correct_damage_with_multiple_percent_additive_modifiers([Range(-1f, 1f, 1f)] float damageValue, [Range(-1f, 1f, .5f)] float modifierValue, [Range(0, 3)] int numberOfModifiers)
        {
            // Arrange
            IDamageModifier[] damageModifiers = new IDamageModifier[numberOfModifiers];

            for (int i = 0; i < numberOfModifiers; i++)
                damageModifiers[i] = new DefaultDamageModifier(modifierValue, IDamageModifier.Type.PercentAdditive);

            // Act
            IDamage damage = Damage.Create((d, t) => new Damage(d, t), damageValue, damageModifiers);

            // Assert
            var modifierValueArray = damageModifiers.Select(mod => mod.Modifier).ToArray();
            float expectedTotalDamage = Mathf.Max(damageValue * (1f + Sum(modifierValueArray)), 0f);
            damage.Amount.Should().Be(expectedTotalDamage);
        }

        [Test]
        public void create_correct_damage_with_multiple_percent_mulitplicative_modifiers([Range(-1f, 1f, 1f)] float damageValue, [Range(-1f, 1f, .5f)] float modifierValue, [Range(0, 3)] int numberOfModifiers)
        {
            // Arrange
            IDamageModifier[] damageModifiers = new IDamageModifier[numberOfModifiers];

            for (int i = 0; i < numberOfModifiers; i++)
                damageModifiers[i] = new DefaultDamageModifier(modifierValue, IDamageModifier.Type.PercentMultiplicative);

            // Act
            IDamage damage = Damage.Create((d, t) => new Damage(d, t), damageValue, damageModifiers);

            // Assert
            var modifierValueArray = damageModifiers.Select(mod => mod.Modifier).ToArray();
            float expectedTotalDamage = Mathf.Max(damageValue * Product(modifierValueArray), 0f);
            damage.Amount.Should().Be(expectedTotalDamage);
        }

        [Test]
        public void create_correct_damage_with_multiple_different_modifiers_1([Range(-1f, 1f, 1f)] float damageValue, [Range(-2f, 2f, .5f)] float modifierValue)
        {
            // Arrange
            int numberOfModifiers = 3;
            IDamageModifier[] damageModifiers = new IDamageModifier[numberOfModifiers];
            IDamageModifier.Type[] modifierTypes = _modifierOrder.SubSet(0, numberOfModifiers);

            for (int i = 0; i < numberOfModifiers; i++)
                damageModifiers[i] = new DefaultDamageModifier(modifierValue, modifierTypes[i]);

            // Act
            IDamage damage = Damage.Create((d, t) => new Damage(d, t), damageValue, damageModifiers);

            // Assert
            //IDamageModifier.Type.Additive,              // 0
            //IDamageModifier.Type.Additive,              // 1
            //IDamageModifier.Type.PercentAdditive,       // 2

            float expectedTotalDamage = damageValue;

            expectedTotalDamage += damageModifiers[0].Modifier;
            expectedTotalDamage += damageModifiers[1].Modifier;
            expectedTotalDamage *= 1f + damageModifiers[2].Modifier;

            damage.Amount.Should().Be(Mathf.Max(expectedTotalDamage, 0));
        }

        [Test]
        public void create_correct_damage_with_multiple_different_modifiers_2([Range(-1f, 1f, 1f)] float damageValue, [Range(-2f, 2f, .5f)] float modifierValue)
        {
            // Arrange
            int numberOfModifiers = 10;
            IDamageModifier[] damageModifiers = new IDamageModifier[numberOfModifiers];
            IDamageModifier.Type[] modifierTypes = _modifierOrder.SubSet(3, numberOfModifiers + 3);

            for (int i = 0; i < numberOfModifiers; i++)
                damageModifiers[i] = new DefaultDamageModifier(modifierValue, modifierTypes[i]);

            // Act
            IDamage damage = Damage.Create((d, t) => new Damage(d, t), damageValue, damageModifiers);

            // Assert
            //IDamageModifier.Type.Additive,              // 0
            //IDamageModifier.Type.Additive,              // 1
            //IDamageModifier.Type.Additive,              // 2
            //IDamageModifier.Type.PercentAdditive,       // 3
            //IDamageModifier.Type.PercentAdditive,       // 4
            //IDamageModifier.Type.PercentAdditive,       // 5
            //IDamageModifier.Type.PercentAdditive,       // 6
            //IDamageModifier.Type.PercentMultiplicative, // 7
            //IDamageModifier.Type.PercentMultiplicative, // 8
            //IDamageModifier.Type.PercentMultiplicative, // 9

            IDamageModifierStack damageModifierStack = new DefaultDamageModifierStack(damageModifiers);
            damageModifierStack.Sort();
            IDamageModifier[] sortedDamageModifiers = damageModifierStack.Modifiers.ToArray();

            float expectedTotalDamage = damageValue;

            float sum = 1f;

            expectedTotalDamage += sortedDamageModifiers[0].Modifier;
            expectedTotalDamage += sortedDamageModifiers[1].Modifier;
            expectedTotalDamage += sortedDamageModifiers[2].Modifier;
            sum += sortedDamageModifiers[3].Modifier;
            sum += sortedDamageModifiers[4].Modifier;
            sum += sortedDamageModifiers[5].Modifier;
            sum += sortedDamageModifiers[6].Modifier;
            expectedTotalDamage *= sum;
            sum = 1f;
            expectedTotalDamage *= sortedDamageModifiers[7].Modifier;
            expectedTotalDamage *= sortedDamageModifiers[8].Modifier;
            expectedTotalDamage *= sortedDamageModifiers[9].Modifier;

            damage.Amount.Should().Be(Mathf.Max(expectedTotalDamage, 0));
        }

        [Test]
        public void create_correct_damage_with_multiple_different_modifiers_3([Range(-1f, 1f, 1f)] float damageValue, [Range(-2f, 2f, .5f)] float modifierValue)
        {
            // Arrange
            int numberOfModifiers = 10;
            IDamageModifier[] damageModifiers = new IDamageModifier[numberOfModifiers];
            IDamageModifier.Type[] modifierTypes = _modifierOrder.SubSet(3, numberOfModifiers + 3);

            for (int i = 0; i < numberOfModifiers; i++)
                damageModifiers[i] = new DefaultDamageModifier(modifierValue, modifierTypes[i], i);

            // Act
            IDamage damage = Damage.Create((d, t) => new Damage(d, t), damageValue, damageModifiers);

            // Assert
            //IDamageModifier.Type.PercentAdditive,       // 0
            //IDamageModifier.Type.PercentMultiplicative, // 1
            //IDamageModifier.Type.PercentAdditive,       // 2
            //IDamageModifier.Type.Additive,              // 3
            //IDamageModifier.Type.PercentMultiplicative, // 4
            //IDamageModifier.Type.PercentMultiplicative, // 5
            //IDamageModifier.Type.PercentAdditive,       // 6
            //IDamageModifier.Type.Additive,              // 7
            //IDamageModifier.Type.PercentAdditive,       // 8
            //IDamageModifier.Type.Additive,              // 9

            IDamageModifierStack damageModifierStack = new DefaultDamageModifierStack(damageModifiers);
            damageModifierStack.Sort();
            IDamageModifier[] sortedDamageModifiers = damageModifierStack.Modifiers.ToArray();

            float expectedTotalDamage = damageValue;

            float sum = 1f;

            sum += sortedDamageModifiers[0].Modifier;
            expectedTotalDamage *= sum;
            sum = 1f;
            expectedTotalDamage *= sortedDamageModifiers[1].Modifier;
            sum += sortedDamageModifiers[2].Modifier;
            expectedTotalDamage *= sum;
            sum = 1f;
            expectedTotalDamage += sortedDamageModifiers[3].Modifier;
            expectedTotalDamage *= sortedDamageModifiers[4].Modifier;
            expectedTotalDamage *= sortedDamageModifiers[5].Modifier;
            sum += sortedDamageModifiers[6].Modifier;
            expectedTotalDamage *= sum;
            sum = 1f;
            expectedTotalDamage += sortedDamageModifiers[7].Modifier;
            sum += sortedDamageModifiers[8].Modifier;
            expectedTotalDamage *= sum;
            expectedTotalDamage += sortedDamageModifiers[9].Modifier;

            damage.Amount.Should().Be(Mathf.Max(expectedTotalDamage, 0));
        }

        [Test]
        public void create_correct_damage_when_using_resistance_modifier(
            [Range(-1f, 1f, 1f)] float damageValue,
            [Range(-2f, 2f, .5f)] float modifierValue,
            [Values(IDamageModifier.Type.Additive, IDamageModifier.Type.PercentAdditive, IDamageModifier.Type.PercentMultiplicative)] IDamageModifier.Type modifierType)
        {
            // Arrange
            IDamageType damageType = new MockDamageType();
            IDamage damage = new Damage(damageValue, damageType);

            IDamageModifier resistanceModifier = new DefaultDamageModifier(modifierValue, modifierType);
            IDamageResistance damageResistance = new DefaultDamageResistance(resistanceModifier, damageType);

            // Act
            IReducedDamage damageAfterResistance = ReducedDamage.Create(damage, damageResistance);

            // Assert
            float expectedAmount = 0f;
            switch (modifierType)
            {
                case IDamageModifier.Type.Additive:
                    expectedAmount = Mathf.Max(damageValue, 0) + modifierValue; // Max here since when creating 'damage' it would do so.
                    break;
                case IDamageModifier.Type.PercentAdditive:
                    expectedAmount = Mathf.Max(damageValue, 0) * (1 + modifierValue); // Max here since when creating 'damage' it would do so.
                    break;
                case IDamageModifier.Type.PercentMultiplicative:
                    expectedAmount = Mathf.Max(damageValue, 0) * modifierValue; // Max here since when creating 'damage' it would do so.
                    break;
            }

            damageAfterResistance.Amount.Should().Be(Mathf.Max(expectedAmount, 0f)); // Max here since when creating 'damageAfterResistance' it would do so.
            damageAfterResistance.DamageType.Should().Be(damageType);
        }

        private float Sum(params float[] values)
        {
            float sum = 0f;

            for (int i = 0; i < values.Length; i++)
                sum += values[i];

            return sum;
        }

        private float Product(params float[] values)
        {
            float product = 1f;

            for (int i = 0; i < values.Length; i++)
                product *= values[i];

            return product;
        }
    }
}
