using Swizzlebake.SimpleAbilitySystem.Game;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    /// <summary>
    /// Represents a basic ability within the Simple Ability System.
    /// Provides core functionality for activation, cooldown, and state management.
    /// </summary>
    public class Ability
    {
        protected float CooldownDuration = 5f;
        protected float Cooldown;
        protected bool IsActive;
        protected bool CanActivate;

        public bool CanActivateAbility()
        {
            return CanActivate && !IsActive;
        }

        /// <summary>
        /// Activates the ability if certain conditions are met.
        /// </summary>
        /// <param name="worldManager">The WorldManager instance managing the game world context.</param>
        /// <param name="owner">The entity that owns and activates the ability.</param>
        /// <returns>True if the ability activation was successful, otherwise false.</returns>
        public virtual bool ActivateAbility(WorldManager worldManager, Entity owner)
        {
            if (!CanActivate)
            {
                return false;
            }
            
            CanActivate = false;
            IsActive = true;
            return true;
        }

        /// <summary>
        /// Updates the ability's internal state on each tick, handling cooldowns and activation logic.
        /// </summary>
        /// <param name="owner">The entity that owns and is affected by the ability.</param>
        /// <param name="dt">The time elapsed since the last tick in seconds.</param>
        /// <returns>True if the ability is active after the update, otherwise false.</returns>
        public virtual bool TickAbility(Entity owner, float dt)
        {
            if (!IsActive)
            {
                Cooldown -= dt;
            }
                
            CanActivate = Cooldown < 0;
            
            return IsActive;
        }

        private void StartCooldown()
        {
            Cooldown = CooldownDuration;
            CanActivate = false;
        }

        protected void EmitTag(Entity owner, string tag)
        {
            owner.OnTagEmitted(new GameTag {Name = tag});
        }

        protected void EndAbility()
        {
            IsActive = false;
            StartCooldown();
        }
    }
}