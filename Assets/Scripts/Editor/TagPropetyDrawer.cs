using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagNameAttribute))]
public class TagPropetyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        EditorGUI.EndProperty();
    }
}
