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
        var CustomDuration = serializedObject.FindProperty("CustomDefense");
        var newDefenseMultiplier = serializedObject.FindProperty("newDefenseMultiplier");
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
    }
}
