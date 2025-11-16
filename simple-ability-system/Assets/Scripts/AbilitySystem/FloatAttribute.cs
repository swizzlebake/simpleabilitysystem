using System;
namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    
    [Serializable]
    public struct FloatAttribute
    {
        public float BaseValue;
        public float FinalValue;
        public float MinValue;
        public float MaxValue;
        public string Name; // Ideally this would not be string based but it is a simple way of handling identification.
        public FloatAttribute(float baseValue, float minValue, float maxValue, string name)
        {
            BaseValue = baseValue;
            FinalValue = BaseValue;
            MinValue = minValue;
            MaxValue = maxValue;
            Name = name;
        }

    }
}