using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(EnemyTransformation))]
[CanEditMultipleObjects]
public class EnemyTransformationEditor : EnemyActionEditor
{

    public override void OnInspectorGUI()
    {
        var myChild = target as EnemyTransformation;
        var EnemyTransformationTargetProperty= serializedObject.FindProperty("EnemyTransformationTarget");
        EditorGUILayout.PropertyField(EnemyTransformationTargetProperty);
        EnemyTransformationTargetProperty.objectReferenceValue= EditorGUILayout.ObjectField("Target Enemy", myChild.EnemyTransformationTarget, typeof(GameObject), false) as GameObject;
        myScript = target as EnemyTransformation;
        base.OnInspectorGUI();
    }
}
