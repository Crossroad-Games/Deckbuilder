using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(Cocoon))]
[CanEditMultipleObjects]
public class CocoonEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        var myCocoon = target as Cocoon;
        var myInfoProperty = serializedObject.FindProperty("myInfo");
        EditorGUILayout.PropertyField(myInfoProperty);
        var CustomizableProperty = serializedObject.FindProperty("Customizable");
        EditorGUILayout.PropertyField(CustomizableProperty);
        var CustomStacks = serializedObject.FindProperty("CustomStacks");
        var CustomShieldMultiplier = serializedObject.FindProperty("CustomShieldMultiplier");
        var newShieldMultiplier = serializedObject.FindProperty("newShieldMultiplier");
        var newStacksCount = serializedObject.FindProperty("newStacksCount");
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myCocoon.Customizable)))
        {
            if (group.visible != false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(CustomShieldMultiplier);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myCocoon.CustomShieldMultiplier)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Shield Multiplier");
                        newShieldMultiplier.floatValue = EditorGUILayout.FloatField(myCocoon.newShieldMultiplier);
                    }
                EditorGUILayout.PropertyField(CustomStacks);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myCocoon.CustomStacks)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("New Stacks");
                        newStacksCount.intValue = EditorGUILayout.IntField(myCocoon.newStacksCount);
                    }
                EditorGUI.indentLevel--;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
