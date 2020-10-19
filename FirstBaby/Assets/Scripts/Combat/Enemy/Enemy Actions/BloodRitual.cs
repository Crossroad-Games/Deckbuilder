using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BloodRitual : EnemyAction
{
    [Header("Action values")]
    [SerializeField] private int BaseShield = 0;
    [SerializeField] private float Multiplier = 4; // Modify this field to multiply damage
    [SerializeField] private float Divider = 1;// Modify this field to divide damage
    [SerializeField] private int AddedShield = 0;// Modify this field to add damage
    [SerializeField] private int SubtractedShield = 0;// Modify this field to subtract damage
    private Dictionary<float,EnemyClass> AlliesHPandShield;
    void Awake()
    {
        BaseShield = myInfo.BaseShield;
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
        AlliesHPandShield.Keys.ToList().Sort();// Order the dictionary from smallest to largest keys
        AlliesHPandShield.Keys.ToList().Reverse();// The first element is now the highest
        var Highest= AlliesHPandShield.Keys.ToList()[0];// ACquires the largest value
        myClass.GainShield((int)(Highest+AddedShield-SubtractedShield)*(int)(Multiplier/Divider));// Gain the largest value as shield
        AlliesHPandShield[Highest].KillMe();// Kill the chosen enemy enemy

    } 
}
