using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AssetsOnlyAttribute))]
public class AssetsOnlyDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();

        property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(GameObject), false);

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(property.serializedObject.targetObject);
    }
}

[CustomPropertyDrawer(typeof(TypeNameAttribute))]
public class TypeNameDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        TypeNameAttribute type = attribute as TypeNameAttribute;

        var list = type.BaseType.FindSubClasses().ToStringList();
        int index = list.IndexOf(property.stringValue);
        if (index < 0)
            index = 0;

        index = EditorGUI.Popup(position, label.text, index, list.ToArray());
        if (list[index] != property.stringValue)
        {
            property.stringValue = list[index];
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }
    }
}