using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PreciseAttack))]
[CanEditMultipleObjects]
public class PreciseAttackEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        myScript = target as PreciseAttack;
        base.OnInspectorGUI();
    }
}
