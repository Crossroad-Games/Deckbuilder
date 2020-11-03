using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BloodRitual : EnemyAction
{
    [Header("Action values")]
    [SerializeField] private int BaseShield = 0;
    private Dictionary<float,EnemyClass> AlliesHPandShield;
    [SerializeField] private float thisMultiplier=2.5f;
    public List<EnemyClass> ImmuneClasses;// Enemy Classes that can't be targeted by this 
    public List<string> ImmuneNames;// Names of the classes that can't be targeted by this
    void Awake()
    {
        BaseShield = myInfo.BaseShield;
        Multiplier = thisMultiplier;// This skill's base multiplier
        foreach (EnemyClass Enemy in ImmuneClasses)// Cycle through the banned classes
            ImmuneNames.Add(Enemy.myData.EnemyName);// Acquire their names and store
    }

    public override IEnumerator Effect()
    {
        AlliesHPandShield = new Dictionary<float, EnemyClass>();
        foreach (EnemyClass Allies in myClass.EnemyManager.CombatEnemies)// Go through all enemies in the scene
        {
            var CanSacrifice = true;
            if (Allies != null && Allies != myClass)// If the enemy is not null and not itself
            {
                foreach (string ImmuneEnemyName in ImmuneNames)// Cycle through all the immune names
                    if (ImmuneEnemyName != string.Empty)// If the name is not empty
                        if (Allies.myData.EnemyName == ImmuneEnemyName)// If it is an immune name
                            CanSacrifice = false;// Can't sacrifice this enemy
                var Value = (Allies.myData.EnemyHP + Allies.myData.EnemyShield);// Acquire the sum of HP and Shield
                if (!AlliesHPandShield.ContainsKey(Value) && CanSacrifice)// If there is a draw in value, prioritize consuming the frontliners
                    AlliesHPandShield.Add(Value, Allies);// Store that enemy's HP+Shield
            }

        }
        var List = AlliesHPandShield.Keys.ToList();// Order the dictionary from smallest to largest keys
        List.Sort();
        List.Reverse();// The first element is now the highest
        var Highest = (int)List[0];// ACquires the largest value
        var ShieldGain = CalculateAction(Highest);
        myClass.GainShield(ShieldGain);// Gain the largest value as shield
        AlliesHPandShield[Highest].KillMe();// Kill the chosen enemy enemy
        while (!ActionDone)
        {
            yield return new WaitForSeconds(1f);
            ActionDone = true;
            Debug.LogWarning("Needs to update this part to get ActionDone from animator and change the delay");
        }
    }
}
