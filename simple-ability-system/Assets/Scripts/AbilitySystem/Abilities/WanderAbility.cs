
using Swizzlebake.SimpleAbilitySystem.Game;
using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities.Abilities
{
    public class WanderAbility : Ability
    {
        public Vector3 TargetPosition;
        public override bool ActivateAbility(WorldManager worldManager, Entity owner)
        {
            if (!base.ActivateAbility(worldManager, owner))
            {
                return false;
            }
            
            TargetPosition = worldManager.GetPositionInRange(owner.transform, owner.AbilitySystem.GetAttributeValue("Range"));

            EmitTag(owner, GameConstants.TagWanderStarted);
            return true;
        }

        public override bool TickAbility(Entity owner, float dt)
        {
            if (!base.TickAbility(owner, dt))
            {
                return false;
            }

            var delta = TargetPosition - owner.transform.position;
            if (delta.magnitude < 0.1f)
            {
                EmitTag(owner, GameConstants.TagWanderEnded);
                EndAbility();
                return false;
            }
            
            delta.Normalize();
            delta *= owner.AbilitySystem.GetAttributeValue("Speed") * dt;
            owner.transform.position += delta;

            return true;
        }
    }
}