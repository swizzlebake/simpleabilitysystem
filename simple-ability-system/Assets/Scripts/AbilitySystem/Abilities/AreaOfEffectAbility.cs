using Swizzlebake.SimpleAbilitySystem.Abilities.Effects;
using Swizzlebake.SimpleAbilitySystem.Game;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    /// <summary>
    /// Represents an ability that applies an area of effect (AoE) to entities within range,
    /// dealing damage to all valid targets except the owner of the ability.
    /// </summary>
    /// <seealso cref="Ability"/>
    public class AreaOfEffectAbility : Ability
    {
        public override bool ActivateAbility(WorldManager worldManager, Entity owner)
        {
            if (!base.ActivateAbility(worldManager, owner))
            {
                return false;
            }
            
            var entities = worldManager.GetEntitiesInRange(owner, owner.AbilitySystem.GetAttributeValue("Range"));
            foreach (var entity in entities)
            {
                if (entity == owner)
                {
                    continue;
                }
                
                entity.AbilitySystem.ApplyEffect(new DamageEffect(owner.AbilitySystem.GetAttributeValue("Damage")), owner.AbilitySystem);
            }

            if (entities.Length > 0)
            {
                EmitTag(owner, GameConstants.TagAttacked);
            }
            
            EndAbility();
            return true;
        }
    }
}