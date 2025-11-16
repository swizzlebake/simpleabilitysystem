using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities.Data
{
    public interface IDataConfig
    {
        public GameObject GetPrefab();
    }
    
    public class DataConfig : ScriptableObject
    {
        public virtual GameObject GetPrefab()
        {
            return null;
        }

        public virtual ITrait[] GetTraits()
        {
            return null;
        }
    }
}