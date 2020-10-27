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
        myScript.myInfo = EditorGUILayout.ObjectField("My Info", myScript.myInfo, typeof(ScriptableObject), false) as EnemyActionInfo;
        myScript.Customizable = GUILayout.Toggle(myScript.Customizable, "Customize"); 
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.Customizable)))
        {  
            if (group.visible != false)
            {
                EditorGUI.indentLevel++;
                myScript.CustomDamage = GUILayout.Toggle(myScript.CustomDamage, "Custom Damage");
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.CustomDamage)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Damage Multiplier");
                        myScript.BaseDamageMultiplier = EditorGUILayout.FloatField(myScript.BaseDamageMultiplier);
                    }
                myScript.CustomShield = GUILayout.Toggle(myScript.CustomShield, "Custom Shield");
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.CustomShield)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Shield Multiplier");
                        myScript.BaseShieldMultiplier = EditorGUILayout.FloatField(myScript.BaseShieldMultiplier);
                    }
                EditorGUI.indentLevel--;
            }
        }
    }
}
