using Swizzlebake.SimpleAbilitySystem.Game;
using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities.Abilities
{
    public class DashAbility : Ability
    {
        public Entity TargetEntity;
        
        public override bool ActivateAbility(WorldManager worldManager, Entity owner)
        {
            if (!base.ActivateAbility(worldManager, owner))
            {
                return false;
            }
            
            var entities = worldManager.GetEntitiesInRange(owner, owner.AbilitySystem.GetAttributeValue("Range"));
            if (entities.Length == 0)
            {
                return false;
            }
            
            TargetEntity = entities[Random.Range(0, entities.Length)];

            EmitTag(owner, GameConstants.TagDashStarted);
            return true;
        }

        public override bool TickAbility(Entity owner, float dt)
        {
            if (!base.TickAbility(owner, dt))
            {
                return false;
            }

            if (!TargetEntity)
            {
                EndAbility();
                return false;
            }
            
            var delta = TargetEntity.transform.position - owner.transform.position;
            if (delta.magnitude < 0.1f)
            {
                EmitTag(owner, GameConstants.TagDashEnded);
                EndAbility();
                return false;
            }
            
            delta.Normalize();
            delta *= owner.AbilitySystem.GetAttributeValue("DashSpeed") * dt;
            owner.transform.position += delta;

            return true;
        }
    }
}