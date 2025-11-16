using System;
using Swizzlebake.SimpleAbilitySystem.Abilities.Abilities;
using Swizzlebake.SimpleAbilitySystem.Abilities.Traits;
using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities.Data
{  
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
            var seekerTrait = new SeekerTrait();
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