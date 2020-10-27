﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BloodRitual : EnemyAction
{
    [Header("Action values")]
    [SerializeField] private int BaseShield = 0;
    private Dictionary<float,EnemyClass> AlliesHPandShield;
    [SerializeField] private float thisMultiplier=2.5f;
    void Awake()
    {
        BaseShield = myInfo.BaseShield;
        Multiplier = thisMultiplier;// This skill's base multiplier
    }
    public override void Effect()
    {
       AlliesHPandShield = new Dictionary<float, EnemyClass>();
       foreach(EnemyClass Allies in myClass.EnemyManager.CombatEnemies)// Go through all enemies in the scene
       {
            if (Allies != null && Allies != myClass)// If the enemy is not null and not itself
            {
                var Value = (Allies.myData.EnemyHP + Allies.myData.EnemyShield);
                if (!AlliesHPandShield.ContainsKey(Value))// If there is a draw in value, prioritize consuming the frontliners
                    AlliesHPandShield.Add(Value, Allies);// Store that enemy's HP+Shield
            }
                
       }
        var List = AlliesHPandShield.Keys.ToList();// Order the dictionary from smallest to largest keys
        List.Sort();
        List.Reverse();// The first element is now the highest
        var Highest=(int) List[0];// ACquires the largest value
        var ShieldGain = CalculateAction(Highest);
        myClass.GainShield(ShieldGain);// Gain the largest value as shield
        AlliesHPandShield[Highest].KillMe();// Kill the chosen enemy enemy

    } 
}
