using System;
namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    /// <summary>
    /// Represents a floating-point attribute used in the ability system.
    /// This struct encapsulates values for the base, final, minimum, and maximum,
    /// along with a name identifier for the attribute.
    /// </summary>
    /// <remarks>
    /// The <c>BaseValue</c> represents the initial value of the attribute,
    /// the <c>FinalValue</c> represents the calculated or modified value,
    /// the <c>MinValue</c> and <c>MaxValue</c> define the boundaries within which the attribute operates,
    /// and the <c>Name</c> provides a way to identify the attribute in the system.
    /// </remarks>
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