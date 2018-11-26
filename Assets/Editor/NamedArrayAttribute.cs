using UnityEngine;

#if UNITY_EDITOR
using System;
using UnityEditor;
#endif

// Defines an attribute that makes the array use enum values as labels.
// Use like this:
//      [NamedArray(typeof(eDirection))] public GameObject[] m_Directions;

[CustomPropertyDrawer(typeof(ResourceTypeENUM))]
public class NamedArrayDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty key = property.FindPropertyRelative("key");
        SerializedProperty value = property.FindPropertyRelative("value");
        GUIContent enumLabel = new GUIContent(key.enumDisplayNames[key.enumValueIndex]);

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), enumLabel);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var unitRect = new Rect(position.x, position.y, position.width, position.height);
        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(unitRect, value);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}