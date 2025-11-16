using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    /// <summary>
    /// Represents a trait within the ability system, encapsulating both a set of attributes
    /// and abilities that define specific characteristics or behaviors.
    /// </summary>
    public interface ITrait
    {
        public AttributeSet GetSet();
        IEnumerable<Ability> GetAbilities();
    }

    /// <summary>
    /// Represents a specific implementation of a trait within the ability system,
    /// including a predefined set of attributes, abilities, and passive effects.
    /// Provides methods to manage and extend abilities, attributes, and passive effects.
    /// </summary>
    /// <typeparam name="TAttributeSet">
    /// The type of attribute set associated with this trait, constrained to inherit from <see cref="AttributeSet"/>.
    /// </typeparam>
    public class Trait<TAttributeSet> : ITrait  where TAttributeSet : AttributeSet, new()
    {
        public TAttributeSet Attributes = new();
        public Ability[] Abilities = Array.Empty<Ability>();
        public Effect[] PassiveEffects = Array.Empty<Effect>();
        
        private int _abilityIndex = 0;
        private int _passiveIndex = 0;

        public void AddAbility(Ability ability)
        {
            if (_abilityIndex + 1 >= Abilities.Length)
            {
                Array.Resize(ref Abilities, Mathf.Max(Abilities.Length * 2, 2));
            }

            Abilities[_abilityIndex++] = ability;
        }
        
        public void AddPassive(Effect effect)
        {
            if (_passiveIndex + 1 >= PassiveEffects.Length)
            {
                Array.Resize(ref PassiveEffects, Mathf.Max(PassiveEffects.Length * 2, 2));
            }

            PassiveEffects[_passiveIndex++] = effect;
        }

        public AttributeSet GetSet()
        {
            return Attributes;
        }

        public IEnumerable<Ability> GetAbilities()
        {
            return Abilities;
        }
    }
}