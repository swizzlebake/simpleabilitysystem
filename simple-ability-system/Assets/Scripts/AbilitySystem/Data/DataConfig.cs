using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities.Data
{
    /// <summary>
    /// Represents a base configuration for ability-related data within the Simple Ability System.
    /// </summary>
    /// <remarks>
    /// This class serves as a ScriptableObject that provides a foundation for creating and managing
    /// ability-related data configurations. It can be extended to define specific configurations
    /// for different abilities or entities. Subclasses should override the provided methods to return
    /// tailored prefabs and traits.
    /// </remarks>
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