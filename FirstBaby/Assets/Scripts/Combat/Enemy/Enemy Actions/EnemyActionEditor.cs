using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(EnemyAttack))]
[CanEditMultipleObjects]
public class EnemyActionEditor : Editor
{
    SerializedProperty myInfo;
    private void OnEnable()
    {
        myInfo = serializedObject.FindProperty("myInfo");
    }
    public override void OnInspectorGUI()
    {
        var myScript = target as EnemyAttack;   
        myScript.myInfo = EditorGUILayout.ObjectField("My Info: ", myScript.myInfo, typeof(ScriptableObject), false) as EnemyActionInfo;
        EditorGUI.indentLevel++;
        myScript.Customizable = GUILayout.Toggle(myScript.Customizable, "Customize"); 
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.Customizable)))
        {
            if (group.visible != false)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PrefixLabel("Base Damage");
                myScript.newBaseDamage = EditorGUILayout.IntField(myScript.newBaseDamage);
                EditorGUILayout.PrefixLabel("Base Shield");
                myScript.newBaseShield = EditorGUILayout.IntField(myScript.newBaseShield);
                EditorGUI.indentLevel--;
            }
        }
    }
}
