using Swizzlebake.SimpleAbilitySystem.Game;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
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

        public void EmitTag(Entity owner, string tag)
        {
            owner.OnTagEmitted(new GameTag {Name = tag});
        }
        
        public void EndAbility()
        {
            IsActive = false;
            StartCooldown();
        }
    }
}