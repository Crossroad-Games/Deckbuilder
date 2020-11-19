using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SoulMaggot : EnemyClass
{
    [SerializeField] private float ConsumeThreshold=.25f;// How much %HP is required to consume another enemy
    [SerializeField] private bool ConsumedEnemy = false, Cocooned=false;
    private int ConsumedEnemyDuration = 0;
    private int CocoonDuration=0;
    public override void EnemyIntention()
    {
        IntendedActions.Clear();
        if(myData.EnemyShield>=15 && ConsumedEnemy)
        {
            IntendedActions.Add(ActionList["Cocoon"]);// Hide inside a cocoon to gain extra shield and defense
        }
        else if (CheckForLowHP())// If an enemy has a really small amount of %HP
        {
            IntendedActions.Add(ActionList["Consume"]);// Consume them
        }
        else
        {
            if(RandomValue<=.3)
                IntendedActions.Add(ActionList["Leech Shield"]);// Steals shield from the player
            else
                IntendedActions.Add(ActionList["Enemy Attack"]);// Deals damage to the player
        }
           
    }
    public override void EndTurn()
    {
        
        base.EndTurn();
        ConsumedEnemyDuration = ConsumedEnemyDuration >= 1 ? ConsumedEnemyDuration - 1 : 0;// Countdown
        ConsumedEnemy = ConsumedEnemyDuration > 0;// True if Consumed enemy is more than 0
        CocoonDuration = CocoonDuration >= 1 ? CocoonDuration - 1 : 0;// Countdown
        if (CocoonDuration <= 0 && Cocooned)// When the cocoon duration is over
        {
            Debug.Log("Incapacitated: " + Incapacitated);
            if (!Incapacitated)// Loses the effect if incapacitated
                StartCoroutine(ActionList["Enemy Transformation"].Effect());// Use this action
            Cocooned = false;// No longer cocooned
        }
    }
    private bool CheckForLowHP()
    {
        if (EnemyManager.CombatEnemies.Count == 1)// If this is the last enemy
        {
            return false;// Return false
        }
        var AlliesHP = new Dictionary<float, EnemyClass>();
        foreach (EnemyClass Allies in EnemyManager.CombatEnemies)// Go through all enemies in the scene
        {
            if (Allies != null && Allies != this)// If the enemy is not null and not itself
            {
                var Value = ((float)Allies.myData.EnemyHP/Allies.myData.EnemyMaxHP);// Get the % of HP each enemy has
                if (!AlliesHP.ContainsKey(Value))// If there is a draw in value, prioritize consuming the frontliners
                    AlliesHP.Add(Value, Allies);// Store that enemy's %HP
            }
        }
        var List = AlliesHP.Keys.ToList();// Order the dictionary from smallest to largest keys
        List.Sort();
        var Highest = List[0];// Acquires the smallest value
        return Highest <= ConsumeThreshold;
       
    }
    public override IEnumerator ActionPhaseCoroutine()
    {
        yield return StartCoroutine(base.ActionPhaseCoroutine());
        Debug.Log("Stuff after the action");
        foreach (EnemyAction Action in TurnActions)// Go through all the actions the enemy intends to perform
            if (Action != null)// Check if its null
            {
                if (Action.ActionName == "Consume")
                    ConsumedEnemy = true;
                if (Action.ActionName == "Cocoon")
                {
                    CocoonDuration = GetComponent<Cocoon>().TurnCount;// Get the duration of the cocoon
                    Debug.Log("Cocoon Duration: "+ CocoonDuration);
                    Cocooned = true;
                }
            }
    }
}
