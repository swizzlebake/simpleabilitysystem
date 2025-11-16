namespace Swizzlebake.SimpleAbilitySystem.Abilities.Effects
{
    public class DamageEffect : Effect
    {
        private readonly float _damage;

        public DamageEffect(float damage)
        {
            _damage = damage;
            _modifiers = new[] { new FloatAttributeModifier { Name = "Health", AddValue = -damage, MultiplyValue = 1 } };
        }
        public override void Applied(AbilitySystem abilitySystem)
        {
        }

        public override void Expired(AbilitySystem abilitySystem)
        {
            
        }
    }
}