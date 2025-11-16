using Swizzlebake.SimpleAbilitySystem.Game;

namespace Swizzlebake.SimpleAbilitySystem.Abilities.Effects
{
    /// <summary>
    /// Represents a specific type of effect that applies damage to a target
    /// in the ability system. This class modifies an entity's attributes
    /// by reducing its health value based on the specified damage amount.
    /// </summary>
    public class DamageEffect : Effect
    {
        public DamageEffect(float damage)
        {
            _modifiers = new[] { new FloatAttributeModifier { Name = GameConstants.AttributeHealth, AddValue = -damage, MultiplyValue = 1 } };
        }
    }
}