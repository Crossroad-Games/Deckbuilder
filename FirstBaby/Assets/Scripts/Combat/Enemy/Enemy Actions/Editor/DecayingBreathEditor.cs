using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(DecayingBreath))]
[CanEditMultipleObjects]
public class DecayingBreathEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        var myDecayingBreath = target as DecayingBreath;
        var myInfoProperty = serializedObject.FindProperty("myInfo");
        EditorGUILayout.PropertyField(myInfoProperty);
        var CustomizableProperty = serializedObject.FindProperty("Customizable");
        EditorGUILayout.PropertyField(CustomizableProperty);
        var CustomStacksProperty = serializedObject.FindProperty("CustomStacks");
        var CustomDamageProperty = serializedObject.FindProperty("CustomDamage");
        var BaseDamageMultiplier = serializedObject.FindProperty("BaseDamageMultiplier");
        var newAmountofStacks = serializedObject.FindProperty("newAmountofStacks");
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myDecayingBreath.Customizable)))
        {
            if (group.visible != false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(CustomStacksProperty);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myDecayingBreath.CustomStacks)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Amount of Stacks");
                        newAmountofStacks.intValue = EditorGUILayout.IntField(myDecayingBreath.newAmountofStacks);
                    }
                EditorGUILayout.PropertyField(CustomDamageProperty);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myDecayingBreath.CustomDamage)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Damage Multiplier");
                        BaseDamageMultiplier.floatValue = EditorGUILayout.FloatField(myDecayingBreath.BaseDamageMultiplier);
                    }
                EditorGUI.indentLevel--;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
