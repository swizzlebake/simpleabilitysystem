using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    public interface ITrait
    {
        public AttributeSet GetSet();
        IEnumerable<Ability> GetAbilities();
    }
    
    public class Trait<TAttributeSet> : ITrait  where TAttributeSet : AttributeSet, new()
    {
        public TAttributeSet Attributes = new();
        public Ability[] Abilities = Array.Empty<Ability>();
        public Effect[] PassiveEffects = Array.Empty<Effect>();
        
        private int _abilityIndex = -1;
        private int _passiveIndex = -1;

        public void AddAbility(Ability ability)
        {
            if (_abilityIndex + 1 >= Abilities.Length)
            {
                Array.Resize(ref Abilities, Mathf.Max(Abilities.Length * 2, 2));
            }

            _abilityIndex++;
            Abilities[_abilityIndex] = ability;
        }
        
        public void AddPassive(Effect effect)
        {
            if (_passiveIndex + 1 >= PassiveEffects.Length)
            {
                Array.Resize(ref PassiveEffects, Mathf.Max(PassiveEffects.Length * 2, 2));
            }

            _passiveIndex++;
            PassiveEffects[_passiveIndex] = effect;
        }

        public void AddAttribute(FloatAttribute attribute)
        {
            Attributes.AddFloatAttribute(attribute);
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