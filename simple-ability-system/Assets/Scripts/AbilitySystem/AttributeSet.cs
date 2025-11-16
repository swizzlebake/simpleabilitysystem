using System;
using UnityEngine;
using UnityEngine.Events;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    [Serializable]
    public struct AttributeSetParams
    {
        public string AttributeName;
        public float PreValue;
        public float PostValue;
    }
    
    public class AttributeSet
    {
        private int _attributeIndex = -1;
        private FloatAttribute[] _floatAttributes = Array.Empty<FloatAttribute>();
        private UnityEvent<AttributeSetParams> _onAttributeChange = new UnityEvent<AttributeSetParams>();

        public void RegisterOnAttributeChange(UnityAction<AttributeSetParams> callback)
        {
            _onAttributeChange.AddListener(callback);
        }
        
        public int AddFloatAttribute(FloatAttribute attribute)
        {
            return AddFloatAttribute(attribute.BaseValue, attribute.MinValue, attribute.MaxValue, attribute.Name);
        }
        
        public int AddFloatAttribute(float baseValue, float minValue, float maxValue, string name)
        {
            if (_attributeIndex + 1 >= _floatAttributes.Length)
            {
                Array.Resize(ref _floatAttributes, Mathf.Max(_floatAttributes.Length * 2, 2));
            }

            _attributeIndex++;
            
            _floatAttributes[_attributeIndex] = new FloatAttribute(baseValue, minValue, maxValue, name);

            return _attributeIndex;
        }

        public virtual void ApplyEffect(EffectParams effectParams)
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
                
                for (int j = 0; j < _floatAttributes.Length; j++)
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

        public virtual void RemoveEffect(EffectParams effectParams)
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
                
                for (int j = 0; j < _floatAttributes.Length; j++)
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
        public virtual void PreAttributeChange(ref AttributeSetParams attributeSetParams, ref FloatAttribute attribute)
        {
            // Apply clamping
            attributeSetParams.PostValue = Mathf.Clamp(attributeSetParams.PostValue, attribute.MinValue, attribute.MaxValue);
        }

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
    }
}
