using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(EnemyAttack))]
[CanEditMultipleObjects]
public class EnemyAttackEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        myScript = target as EnemyAttack;
        base.OnInspectorGUI();
    }
}
