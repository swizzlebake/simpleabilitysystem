using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    /// <summary>
    /// Represents the parameters required to apply an effect within the ability system.
    /// These parameters include the effect to be applied, the system that initiated the effect,
    /// and the target system to which the effect is applied.
    /// </summary>
    public struct EffectParams
    {
        public Effect Effect;
        public AbilitySystem Instigator;
        public AbilitySystem Target;
    }

    /// <summary>
    /// Represents a modifier that adjusts a specific float attribute within the ability system.
    /// The adjustment can include an additive value, a multiplicative value, or both,
    /// and is identified by the name of the attribute being modified.
    /// </summary>
    public struct FloatAttributeModifier
    {
        public string Name;
        public float AddValue;
        public float MultiplyValue;
    }

    /// <summary>
    /// Represents a base class for effects within the ability system.
    /// Effects are used to apply specific changes or behaviors to an
    /// AbilitySystem, including attribute modifications or custom logic.
    /// </summary>
    public class Effect
    {
        protected FloatAttributeModifier[] _modifiers;
        public FloatAttributeModifier[] Modifiers => _modifiers;
        private float _duration;
        public float Duration => _duration;

        public virtual void Applied(AbilitySystem abilitySystem)
        {

        }

        public virtual void Expired(AbilitySystem abilitySystem)
        {
        }
    }
}
