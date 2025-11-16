using System;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    [Serializable]
    public struct GameTag : IEquatable<GameTag>
    {
        public string Name;
        public static bool operator==(GameTag tag, string name)
        {
            if (string.IsNullOrEmpty(tag.Name) && string.IsNullOrEmpty(name))
            {
                return true;
            }

            if (string.IsNullOrEmpty(tag.Name) || string.IsNullOrEmpty(name))
            {
                return false;
            }
            
            return tag.Name.Equals(name);
        }

        public static bool operator !=(GameTag tag, string name) => !(tag == name);

        public bool Equals(GameTag other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is GameTag other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}