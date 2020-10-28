using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(LeechShield))]
[CanEditMultipleObjects]
public class LeechShieldEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        myScript = target as LeechShield;
        base.OnInspectorGUI();
    }
}
