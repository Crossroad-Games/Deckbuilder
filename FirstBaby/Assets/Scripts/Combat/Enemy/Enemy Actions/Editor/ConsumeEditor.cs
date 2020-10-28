using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(Consume))]
[CanEditMultipleObjects]
public class ConsumeEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        var myConsume = target as Consume;
        myConsume.myInfo = EditorGUILayout.ObjectField("My Info", myConsume.myInfo, typeof(ScriptableObject), false) as EnemyActionInfo;
        myConsume.Customizable = GUILayout.Toggle(myConsume.Customizable, "Customize");
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myConsume.Customizable)))
        {
            if (group.visible != false)
            {
                EditorGUI.indentLevel++;
                myConsume.CustomShield = GUILayout.Toggle(myConsume.CustomShield, "Custom Shield");
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myConsume.CustomShield)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Shield Multiplier");
                        myConsume.newShieldMultiplier = EditorGUILayout.FloatField(myConsume.newShieldMultiplier);
                    }
                EditorGUI.indentLevel--;
            }
        }
    }
}
