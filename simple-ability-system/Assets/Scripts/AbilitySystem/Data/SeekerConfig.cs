using System;
using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities.Data
{  
    /// <summary>
    /// Seeker Attribute Data
    /// </summary>
    [Serializable]
    public struct SeekerData
    {
        public FloatAttribute Health;
        public FloatAttribute Range;
        public FloatAttribute Damage;
        public FloatAttribute Speed;
        public FloatAttribute DashSpeed;
        
        public GameObject Prefab;
    }

    /// <summary>
    /// A configuration class used to define the data and behavior of a seeker entity within the Simple Ability System.
    /// </summary>
    [CreateAssetMenu(fileName = "SeekerData", menuName = "AbilitySystem/SeekerData")]
    public class SeekerConfig : DataConfig
    {
        public SeekerData Data;

        public override GameObject GetPrefab()
        {
            return Data.Prefab;
        }

        public override ITrait[] GetTraits()
        {
            var seekerTrait = new Trait<AttributeSet>();
            seekerTrait.AddAbility(new WanderAbility());
            seekerTrait.AddAbility(new DashAbility());
            seekerTrait.AddAbility(new AreaOfEffectAbility());

            seekerTrait.Attributes.AddFloatAttribute(Data.Health);
            seekerTrait.Attributes.AddFloatAttribute(Data.Range);
            seekerTrait.Attributes.AddFloatAttribute(Data.Damage);
            seekerTrait.Attributes.AddFloatAttribute(Data.Speed);
            seekerTrait.Attributes.AddFloatAttribute(Data.DashSpeed);
            
            return new ITrait[] { seekerTrait };
        }
    }
}