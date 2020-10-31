using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(EnemyAction))]
[CanEditMultipleObjects]
public class EnemyActionEditor : Editor
{
    protected EnemyAction myScript;
    public override void OnInspectorGUI()
    {
        myScript = target as EnemyAction;
        var myInfoProperty= serializedObject.FindProperty("myInfo");
        EditorGUILayout.PropertyField(myInfoProperty);
        var CustomizableProperty = serializedObject.FindProperty("Customizable");
        var CustomDamageProperty = serializedObject.FindProperty("CustomDamage");
        var CustomShieldProperty = serializedObject.FindProperty("CustomShield");
        var BaseDamageMultiplier = serializedObject.FindProperty("BaseDamageMultiplier");
        var BaseShieldMultiplier = serializedObject.FindProperty("BaseShieldMultiplier");
        EditorGUILayout.PropertyField(CustomizableProperty);
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.Customizable)))
        {
            if (group.visible != false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(CustomDamageProperty);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.CustomDamage)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Damage Multiplier");
                        BaseDamageMultiplier.floatValue = EditorGUILayout.FloatField(myScript.BaseDamageMultiplier);
                        
                    }
                EditorGUILayout.PropertyField(CustomShieldProperty);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.CustomShield)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Shield Multiplier");
                        BaseShieldMultiplier.floatValue = EditorGUILayout.FloatField(myScript.BaseShieldMultiplier);
                    }
                EditorGUI.indentLevel--;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
        
}
