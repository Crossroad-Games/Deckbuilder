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
        var CustomDefense = serializedObject.FindProperty("CustomDefense");
        var CustomDuration = serializedObject.FindProperty("CustomDuration");
        var CustomShieldMultiplier = serializedObject.FindProperty("CustomShieldMultiplier");
        var newDefenseMultiplier = serializedObject.FindProperty("newDefenseMultiplier");
        var newShieldMultiplier = serializedObject.FindProperty("newShieldMultiplier");
        var newTurnCount = serializedObject.FindProperty("newTurnCount");
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myCocoon.Customizable)))
        {
            if (group.visible != false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(CustomDefense);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myCocoon.CustomDefense)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Defense Multiplier");
                        newDefenseMultiplier.floatValue = EditorGUILayout.FloatField(myCocoon.newDefenseMultiplier);
                    }
                EditorGUILayout.PropertyField(CustomShieldMultiplier);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myCocoon.CustomShieldMultiplier)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Shield Multiplier");
                        newShieldMultiplier.floatValue = EditorGUILayout.FloatField(myCocoon.newShieldMultiplier);
                    }
                EditorGUILayout.PropertyField(CustomDuration);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myCocoon.CustomDuration)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("New Duration");
                        newTurnCount.intValue = EditorGUILayout.IntField(myCocoon.newTurnCount);
                    }
                EditorGUI.indentLevel--;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
