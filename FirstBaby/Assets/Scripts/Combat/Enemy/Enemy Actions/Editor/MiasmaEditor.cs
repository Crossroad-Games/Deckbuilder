using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(Miasma))]
[CanEditMultipleObjects]
public class MiasmaEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        var myMiasma = target as Miasma;
        var myInfoProperty = serializedObject.FindProperty("myInfo");
        EditorGUILayout.PropertyField(myInfoProperty);
        var CustomizableProperty = serializedObject.FindProperty("Customizable");
        EditorGUILayout.PropertyField(CustomizableProperty);
        var CustomizeDisruptDuration = serializedObject.FindProperty("CustomDisruptDuration");
        var CustomDecayStacks = serializedObject.FindProperty("CustomDecayStacks");
        var CustomBurnStacks = serializedObject.FindProperty("CustomBurnStacks");
        var newDisruptDuration = serializedObject.FindProperty("newDisruptDuration");
        var newAmountofDecayStacks = serializedObject.FindProperty("newAmountofDecayStacks");
        var newAmountofBurnStacks = serializedObject.FindProperty("newAmountofBurnStacks");
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myMiasma.Customizable)))
        {
            if (group.visible != false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(CustomBurnStacks);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myMiasma.CustomBurnStacks)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Burn Stacks");
                        newAmountofBurnStacks.intValue = EditorGUILayout.IntField(myMiasma.newAmountofBurnStacks);
                    }
                EditorGUILayout.PropertyField(CustomDecayStacks);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myMiasma.CustomDecayStacks)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Decay Stacks");
                        newAmountofDecayStacks.intValue = EditorGUILayout.IntField(myMiasma.newAmountofDecayStacks);
                    }
                EditorGUILayout.PropertyField(CustomizeDisruptDuration);
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myMiasma.CustomDisruptDuration)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("New Disrupt Duration");
                        newDisruptDuration.intValue = EditorGUILayout.IntField(myMiasma.newDisruptDuration);
                    }
                EditorGUI.indentLevel--;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
