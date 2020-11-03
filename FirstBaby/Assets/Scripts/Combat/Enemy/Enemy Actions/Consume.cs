using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Consume : EnemyAction
{
    private float ShieldMultiplier = 20;
    [SerializeField] public float newShieldMultiplier = 0;// Modified on the custom editor
    protected override void Start()
    {
        base.Start();
        if(Customizable)
            if(CustomShield)
                ShieldMultiplier *= newShieldMultiplier;// Increase or decrease the multiplier on the custom editor
    }
    private EnemyClass CheckForLowHP()// Consumes the enemy with the least amount of HP
    {
        var AlliesHP= new Dictionary<float, EnemyClass>();
        foreach (EnemyClass Allies in myClass.EnemyManager.CombatEnemies)// Go through all enemies in the scene
        {
            if (Allies != null && Allies != this)// If the enemy is not null and not itself
            {
                var Value = ((float)Allies.myData.EnemyHP / Allies.myData.EnemyMaxHP);// Get the % of HP each enemy has
                if (!AlliesHP.ContainsKey(Value))// If there is a draw in value, prioritize consuming the frontliners
                    AlliesHP.Add(Value, Allies);// Store that enemy's %HP
            }

        }
        var List = AlliesHP.Keys.ToList();// Order the dictionary from smallest to largest keys
        List.Sort();
        Debug.Log(List[0]);
        return AlliesHP[List[0]];// Acquire the enemy this attack will consume
    }

    public override IEnumerator Effect()
    {
        var EnemyToConsume = CheckForLowHP();// Acquires the enemy with the least amount of %HP
        myClass.GainShield((int)(CalculateAction(EnemyToConsume.myData.EnemyHP) * ShieldMultiplier));// Converts their HP into shield
        Debug.Log(ShieldMultiplier);
        EnemyToConsume.KillMe();// Kill that enemy and trigger its death event
        while (!ActionDone)
        {
            yield return new WaitForSeconds(1f);
            ActionDone = true;
            Debug.LogWarning("Needs to update this part to get ActionDone from animator and change the delay");
        }
    }
}
