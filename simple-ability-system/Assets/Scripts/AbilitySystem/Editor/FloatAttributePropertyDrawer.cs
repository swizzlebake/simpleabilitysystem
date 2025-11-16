using UnityEditor;
using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Abilities.Editor
{
    /// <summary>
    /// Custom property drawer for the <see cref="FloatAttribute"/> struct.
    /// This drawer provides a customized GUI layout for editing fields of FloatAttribute in the Unity Editor.
    /// </summary>
    [CustomPropertyDrawer(typeof(FloatAttribute))]
    public class FloatAttributePropertyDrawer : PropertyDrawer
    {
        private GUIContent[] _floatLabels = new GUIContent[] { new("Base"), new("Min"), new("Max") };

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight*2;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var baseValue = property.FindPropertyRelative("BaseValue");
            var minValue = property.FindPropertyRelative("MinValue");
            var maxValue = property.FindPropertyRelative("MaxValue");
            var name = property.FindPropertyRelative("Name");
            var singleHeightSize = new Vector2(position.size.x, EditorGUIUtility.singleLineHeight);

            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 100f;
            name.stringValue = EditorGUI.TextField(new Rect(position.position, singleHeightSize), "Name", name.stringValue);
            
            var indentLevel = (EditorGUI.indentLevel + 1)*15f;
            var indentedRect = new Rect(position.position.x + indentLevel, position.position.y + EditorGUIUtility.singleLineHeight, position.size.x- indentLevel, EditorGUIUtility.singleLineHeight);
            var floats = new[] { baseValue.floatValue, minValue.floatValue, maxValue.floatValue };
            EditorGUI.MultiFloatField(indentedRect, _floatLabels, floats);
            
            baseValue.floatValue = Mathf.Clamp(floats[0], floats[1], floats[2]);
            minValue.floatValue = floats[1];
            maxValue.floatValue = floats[2];
            
            EditorGUIUtility.labelWidth = labelWidth;
        }
    }
}