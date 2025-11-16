using System.Collections.Generic;
using Swizzlebake.SimpleAbilitySystem.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    public class AbilitySystem
    {
        private const int InitialEffectCapacity = 4;

        private readonly List<ITrait> _traits = new();
        private readonly List<(EffectParams effect, float duration)> _activeEffects = new(InitialEffectCapacity);

        /// <summary>
        /// Updates all traits and their abilities
        /// </summary>
        public void Update(WorldManager worldManager, Entity owner, float dt)
        {
            foreach (var trait in _traits)
            {
                ProcessTraitAbilities(trait, worldManager, owner, dt);
            }
        }

        /// <summary>
        /// Performs cleanup of expired effects
        /// </summary>
        public void LateUpdate() => ClearExpiredEffects();

        /// <summary>
        /// Adds multiple traits to the ability system
        /// </summary>
        public void AddTraits(IEnumerable<ITrait> traits)
        {
            foreach (var trait in traits)
            {
                _traits.Add(trait);
            }
        }

        /// <summary>
        /// Applies an effect to this ability system
        /// </summary>
        public void ApplyEffect(Effect effect, AbilitySystem instigator)
        {
            if (effect == null) return;

            var effectParams = CreateAndAddEffect(effect, instigator);
            ApplyEffectToTraits(effectParams);
            effect.Applied(this);
        }

        /// <summary>
        /// Gets the value of a named attribute from traits
        /// </summary>
        public float GetAttributeValue(string attributeName)
        {
            foreach (var trait in _traits)
            {
                var (success, value) = trait.GetSet().GetFloatAttribute(attributeName);
                if (success) return value;
            }

            Debug.LogWarning($"Attribute '{attributeName}' not found in any trait!");
            return 0.0f;
        }

        /// <summary>
        /// Registers a callback for attribute changes
        /// </summary>
        public void RegisterAttributeChange(UnityAction<AttributeSetParams> action)
        {
            foreach (var trait in _traits)
            {
                trait.GetSet().RegisterOnAttributeChange(action);
            }
        }

        private void ProcessTraitAbilities(ITrait trait, WorldManager worldManager, Entity owner, float dt)
        {
            foreach (var ability in trait.GetAbilities())
            {
                if (ability == null) continue;

                if (ability.CanActivateAbility())
                {
                    ability.ActivateAbility(worldManager, owner);
                }

                ability.TickAbility(owner, dt);
            }
        }

        private EffectParams CreateAndAddEffect(Effect effect, AbilitySystem instigator)
        {
            var effectParams = new EffectParams
            {
                Effect = effect,
                Instigator = instigator,
                Target = this
            };

            _activeEffects.Add(new (effectParams, effect.Duration));
            return effectParams;
        }

        private void ApplyEffectToTraits(EffectParams effectParams)
        {
            foreach (var trait in _traits)
            {
                trait.GetSet().ApplyEffect(effectParams);
            }
        }

        private void ClearExpiredEffects()
        {
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                if (_activeEffects[i].duration <= 0)
                {
                    foreach (var trait in _traits)
                    {
                        trait.GetSet().RemoveEffect(_activeEffects[i].effect);
                    }
                    
                    _activeEffects[i].effect.Effect.Expired(this);
                    _activeEffects.RemoveAt(i);
                }
            }
        }
    }
}