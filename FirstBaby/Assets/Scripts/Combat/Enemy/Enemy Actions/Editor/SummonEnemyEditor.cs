using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(SummonEnemy))]
[CanEditMultipleObjects]
public class SummonEnemyEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        var myChild = target as SummonEnemy;
        var SummonList = serializedObject.FindProperty("EnemiesToSummon");
        EditorGUILayout.PropertyField(SummonList);
        myScript = target as SummonEnemy;
        base.OnInspectorGUI();
    }
}
