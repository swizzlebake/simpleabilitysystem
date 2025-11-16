using System;
using UnityEngine;
using UnityEngine.Events;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    /// <summary>
    /// Represents the parameters associated with changes to an attribute
    /// in the ability system. Encapsulates the attribute name, its value
    /// before change, and its value after change.
    /// </summary>
    [Serializable]
    public struct AttributeSetParams
    {
        public string AttributeName;
        public float PreValue;
        public float PostValue;
    }

    /// <summary>
    /// Represents a collection of attributes that can be modified and monitored
    /// within the ability system. Provides functionalities to manage attributes
    /// and apply or remove effects adjusting these values.
    /// </summary>
    public class AttributeSet
    {
        private int _attributeIndex = 0;
        private FloatAttribute[] _floatAttributes = Array.Empty<FloatAttribute>();
        private UnityEvent<AttributeSetParams> _onAttributeChange = new UnityEvent<AttributeSetParams>();

        /// <summary>
        /// Registers a callback to be invoked when an attribute changes.
        /// </summary>
        /// <param name="callback">The callback to invoke when an attribute change occurs. It receives an <see cref="AttributeSetParams"/> detailing the attribute change.</param>
        public void RegisterOnAttributeChange(UnityAction<AttributeSetParams> callback)
        {
            _onAttributeChange.AddListener(callback);
        }

        /// <summary>
        /// Adds a new float attribute to the attribute set.
        /// </summary>
        /// <param name="attribute">The float attribute to add, containing its base value, min value, max value, and name.</param>
        public void AddFloatAttribute(FloatAttribute attribute)
        {
            AddFloatAttribute(attribute.BaseValue, attribute.MinValue, attribute.MaxValue, attribute.Name);
        }

        private void AddFloatAttribute(float baseValue, float minValue, float maxValue, string name)
        {
            if (_attributeIndex + 1 >= _floatAttributes.Length)
            {
                Array.Resize(ref _floatAttributes, Mathf.Max(_floatAttributes.Length * 2, 2));
            }
            
            _floatAttributes[_attributeIndex++] = new FloatAttribute(baseValue, minValue, maxValue, name);
        }

        /// <summary>
        /// Applies an effect to the attributes in the attribute set.
        /// </summary>
        /// <param name="effectParams">The parameters of the effect to apply, including the effect modifiers and
        /// the instigator and target of the effect.</param>
        public void ApplyEffect(EffectParams effectParams)
        {
            if (effectParams.Effect == null)
            {
                return;
            }
            
            for (int i = 0; i < effectParams.Effect.Modifiers.Length; i++)
            {
                var addValue = effectParams.Effect.Modifiers[i].AddValue;
                var multiplyValue = effectParams.Effect.Modifiers[i].MultiplyValue;
                
                if(Mathf.Approximately(multiplyValue, 0)) continue;
                
                for (int j = 0; j < _attributeIndex; j++)
                {
                    if (string.IsNullOrEmpty(_floatAttributes[j].Name))
                    {
                        continue;
                    }
                    
                    if (_floatAttributes[j].Name.Equals(effectParams.Effect.Modifiers[i].Name,
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        var setParams = new AttributeSetParams()
                        {
                            AttributeName = _floatAttributes[j].Name,
                            PreValue = _floatAttributes[j].FinalValue,
                            PostValue = (_floatAttributes[j].FinalValue + addValue) * multiplyValue
                        };
                        PreAttributeChange(ref setParams, ref _floatAttributes[j]);
                        _floatAttributes[j].FinalValue = setParams.PostValue;
                        _onAttributeChange.Invoke(setParams);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes an applied effect from the attribute set, reversing the modifications made by the effect.
        /// </summary>
        /// <param name="effectParams">Parameters of the effect to be removed, including the effect details, instigator, and target.</param>
        public void RemoveEffect(EffectParams effectParams)
        {
            if (effectParams.Effect == null)
            {
                return;
            }
            
            for (int i = 0; i < effectParams.Effect.Modifiers.Length; i++)
            {
                var addValue = effectParams.Effect.Modifiers[i].AddValue;
                var multiplyValue = effectParams.Effect.Modifiers[i].MultiplyValue;
                
                if(Mathf.Approximately(multiplyValue, 0)) continue;
                
                for (int j = 0; j < _attributeIndex; j++)
                {
                    
                    if (_floatAttributes[j].Name.Equals(effectParams.Effect.Modifiers[i].Name,
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        _floatAttributes[j].FinalValue /= multiplyValue;
                        _floatAttributes[j].FinalValue -= addValue;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the final value of a float attribute specified by its name.
        /// </summary>
        /// <param name="attributeName">The name of the float attribute to retrieve. Case-insensitive.</param>
        /// <returns>A tuple containing a success flag and the value of the float attribute.
        /// The success flag is true if the attribute is found, otherwise false.
        /// The value is the final value of the attribute if found, otherwise 0.0f.</returns>
        public (bool success, float value) GetFloatAttribute(string attributeName)
        {
            foreach (var attribute in _floatAttributes)
            {
                if (string.IsNullOrEmpty(attribute.Name))
                {
                    continue;
                }
                
                if (attribute.Name.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (true, attribute.FinalValue);
                }
            }

            return (false, 0.0f);
        }
        
        private void PreAttributeChange(ref AttributeSetParams attributeSetParams, ref FloatAttribute attribute)
        {
            // Apply clamping
            attributeSetParams.PostValue = Mathf.Clamp(attributeSetParams.PostValue, attribute.MinValue, attribute.MaxValue);
        }

    }
}
