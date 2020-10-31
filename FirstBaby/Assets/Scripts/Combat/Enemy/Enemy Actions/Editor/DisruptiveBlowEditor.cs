using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(DisruptiveBlow))]
[CanEditMultipleObjects]
public class DisruptiveBlowEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        var myDisruptiveBlow = target as DisruptiveBlow;
        var myInfoProperty = serializedObject.FindProperty("myInfo");
        EditorGUILayout.PropertyField(myInfoProperty);
        var CustomizableProperty = serializedObject.FindProperty("Customizable");
        EditorGUILayout.PropertyField(CustomizableProperty);
        var CustomizeShieldDepletion = serializedObject.FindProperty("CustomShieldDepletion");
        var CustomizeDuration = serializedObject.FindProperty("CustomDuration");
        var NewTurnCount = serializedObject.FindProperty("newTurnCount");
        var NewDepletion = serializedObject.FindProperty("newDepletion");
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myDisruptiveBlow.Customizable)))
        {
            if (group.visible != false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(CustomizeShieldDepletion);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myDisruptiveBlow.CustomShieldDepletion)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Depletion Multiplier");
                        NewDepletion.floatValue = EditorGUILayout.FloatField(myDisruptiveBlow.newDepletion);
                    }
                EditorGUILayout.PropertyField(CustomizeDuration);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myDisruptiveBlow.CustomDuration)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("New Duration");
                        NewTurnCount.intValue = EditorGUILayout.IntField(myDisruptiveBlow.newTurnCount);
                    }
                EditorGUI.indentLevel--;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
