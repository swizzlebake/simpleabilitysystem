using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    public struct EffectParams
    {
        public Effect Effect;
        public AbilitySystem Instigator;
        public AbilitySystem Target;
    }

    public struct FloatAttributeModifier
    {
        public string Name;
        public float AddValue;
        public float MultiplyValue;
    }

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
