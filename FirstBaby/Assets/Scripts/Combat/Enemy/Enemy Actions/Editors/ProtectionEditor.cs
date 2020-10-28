using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(Protection))]
[CanEditMultipleObjects]
public class ProtectionEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        var myScript = target as Protection;
        base.OnInspectorGUI();
    }
}
