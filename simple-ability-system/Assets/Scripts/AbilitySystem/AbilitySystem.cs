using System;
using Swizzlebake.SimpleAbilitySystem.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    public class AbilitySystem
    {
        public (EffectParams effectParams, float duration)[] ActiveEffects = new (EffectParams effect, float duration)[1];
        private int _traitIndex = 0;
        private ITrait[] _traits = Array.Empty<ITrait>();
        
        public void Update(WorldManager worldManager, Entity owner, float dt)
        {
            foreach (var trait in _traits)
            {
                if (trait == null)
                {
                    continue;
                }
                
                foreach (var ability in trait.GetAbilities())
                {
                    if (ability == null)
                    {
                        continue;
                    }

                    if (ability.CanActivateAbility())
                    {
                        ability.ActivateAbility(worldManager, owner);
                    }
                    
                    ability.TickAbility(owner, dt);
                }
            }
        }

        public void LateUpdate()
        {
            ClearExpiredEffects();
        }

        public void AddTraits(ITrait[] traits)
        {
            foreach (var trait in traits)
            {
                AddTrait(trait);
            }
        }
        
        void AddTrait(ITrait trait)
        {
            if (_traitIndex + 1 >= _traits.Length)
            {
                Array.Resize(ref _traits, Mathf.Max(_traits.Length * 2, 2));
            }

            _traits[_traitIndex++] = trait;
        }
        
        public void ApplyEffect(Effect effect, AbilitySystem instigator)
        {
            var effectParams = AddEffect(effect, instigator);
            for (int i = 0; i < _traitIndex; i++)
            {
                var set = _traits[i].GetSet();
                set.ApplyEffect(effectParams);
            }
            
            effectParams.Effect.Applied(this);
        }
        
        EffectParams AddEffect(Effect effect, AbilitySystem instigator)
        {
            var effectParams = new EffectParams() { Effect = effect, Instigator = instigator, Target = this };
            for (int i = 0; i < ActiveEffects.Length; i++)
            {
                if (ActiveEffects[i].effectParams.Effect == null)
                {
                    ActiveEffects[i] = (effectParams, effect.Duration);
                    return effectParams;
                }
            }

            var length = ActiveEffects.Length;
            Array.Resize(ref ActiveEffects, Mathf.Max(ActiveEffects.Length * 2, 2));
            ActiveEffects[length] = (effectParams, effect.Duration);
            return effectParams;
        }

        void ClearExpiredEffects()
        {
            for (int i = 0; i < ActiveEffects.Length; i++)
            {
                if (ActiveEffects[i].effectParams.Effect != null && ActiveEffects[i].duration <= 0)
                {
                    ActiveEffects[i].effectParams.Effect.Expired(this);
                    ActiveEffects[i].effectParams = default;
                    ActiveEffects[i].duration = 0;
                }
            }
        }

        public float GetAttributeValue(string attributeName)
        {
            foreach (var trait in _traits)
            {
                if (trait == null)
                {
                    continue;
                }
                
                var (success, value) = trait.GetSet().GetFloatAttribute(attributeName);
                if (success)
                {
                    return value;
                }
            }

            Debug.LogWarning("The attribute " + attributeName + " was not found in any trait!");
            return 0.0f;
        }

        public void RegisterAttributeChange(UnityAction<AttributeSetParams> action)
        {
            for (int i = 0; i < _traitIndex; i++)
            {
               _traits[i].GetSet().RegisterOnAttributeChange(action);
            }
        }
    }
}