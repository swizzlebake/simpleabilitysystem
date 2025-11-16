using System;
using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities.Data
{
    /// <summary>
    /// Bomber Attribute Data
    /// </summary>
    [Serializable]
    public struct BomberData
    {
        public FloatAttribute Health;
        public FloatAttribute Range;
        public FloatAttribute Damage;
        public FloatAttribute Speed;
        
        public GameObject Prefab;
    }

    /// <summary>
    /// Represents a configuration for the Bomber entity in the Simple Ability System.
    /// </summary>
    [CreateAssetMenu(fileName = "BomberData", menuName = "AbilitySystem/BomberData")]
    public class BomberConfig : DataConfig
    {
        public BomberData Data;

        public override GameObject GetPrefab()
        {
            return Data.Prefab;
        }

        public override ITrait[] GetTraits()
        {
            var bomberTrait = new Trait<AttributeSet>();
            bomberTrait.AddAbility(new WanderAbility());
            bomberTrait.AddAbility(new AreaOfEffectAbility());

            bomberTrait.Attributes.AddFloatAttribute(Data.Health);
            bomberTrait.Attributes.AddFloatAttribute(Data.Range);
            bomberTrait.Attributes.AddFloatAttribute(Data.Damage);
            bomberTrait.Attributes.AddFloatAttribute(Data.Speed);
            
            return new ITrait[] { bomberTrait };
        }
    }
}