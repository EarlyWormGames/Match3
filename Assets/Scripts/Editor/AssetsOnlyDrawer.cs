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
        if (property == null)
            return;

        EditorGUI.BeginChangeCheck();

        property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(GameObject), false);

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(property.serializedObject.targetObject);
    }
}