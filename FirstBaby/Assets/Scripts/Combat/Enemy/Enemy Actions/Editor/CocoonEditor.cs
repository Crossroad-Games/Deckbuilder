using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(Cocoon))]
[CanEditMultipleObjects]
public class CocoonEditor : EnemyActionEditor
{
    public override void OnInspectorGUI()
    {
        var myCocoon = target as Cocoon;
        myCocoon.myInfo = EditorGUILayout.ObjectField("My Info", myCocoon.myInfo, typeof(ScriptableObject), false) as EnemyActionInfo;
        myCocoon.Customizable = GUILayout.Toggle(myCocoon.Customizable, "Customize");
        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myCocoon.Customizable)))
        {
            if (group.visible != false)
            {
                EditorGUI.indentLevel++;
                myCocoon.CustomDefense = GUILayout.Toggle(myCocoon.CustomDefense, "Custom Defense");
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myCocoon.CustomDefense)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("Defense Multiplier");
                        myCocoon.newDefenseMultiplier = EditorGUILayout.FloatField(myCocoon.newDefenseMultiplier);
                    }
                myCocoon.CustomDuration = GUILayout.Toggle(myCocoon.CustomDuration, "Custom Duration");
                using (var Value = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myCocoon.CustomDuration)))
                    if (Value.visible != false)
                    {
                        EditorGUILayout.PrefixLabel("New Duration");
                        myCocoon.newTurnCount = EditorGUILayout.IntField(myCocoon.newTurnCount);
                    }
                EditorGUI.indentLevel--;
            }
        }
    }
}
