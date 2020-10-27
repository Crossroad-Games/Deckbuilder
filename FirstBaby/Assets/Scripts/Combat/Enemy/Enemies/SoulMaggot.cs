using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SoulMaggot : EnemyClass
{
    [SerializeField] private float ConsumeThreshold=.25f;// How much %HP is required to consume another enemy
    private bool ConsumedEnemy = false, Cocooned=false;
    public override void EnemyIntention()
    {
        IntendedActions.Clear();
        if(myData.EnemyShield>=20 && ConsumedEnemy)
        {
            IntendedActions.Add(ActionList["Cocoon"]);// Hide inside a cocoon to gain extra shield and defense
        }
        else if (CheckForLowHP())// If an enemy has a really small amount of %HP
        {
            IntendedActions.Add(ActionList["Consume"]);// Consume them
        }
        else
        {
            if(RandomValue<=.7)
                IntendedActions.Add(ActionList["Leech Shield"]);// Steals shield from the player
            else
                IntendedActions.Add(ActionList["Enemy Attack"]);// Deals damage to the player
        }
           
    }
    public override void ActionPhase()
    {
        if (TurnManager.State == CombatState.EnemyActionPhase)
        {
            EnemyIntention();// Checks what the enemy is going to do
            if (ConsumedEnemy)// If the maggot has consumed a unit in the previous turn
                ConsumedEnemy = false;// No longer considered able to cocoon
            foreach (EnemyAction Action in IntendedActions)// Go through all the actions the enemy intends to perform
                if (Action != null && !Incapacitated)// Check if its null
                {
                    Action.Effect();// Executes this action's effect
                    if (Action.ActionName == "Consume")
                        ConsumedEnemy = true;
                    if (Action.ActionName == "Cocoon")
                        Cocooned = true;
                }
            EndTurn();// End its turn
        }
    }
    private bool CheckForLowHP()
    {
        if (EnemyManager.CombatEnemies.Count == 1)// If this is the last enemy
            return false;// Return false
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
        return Highest <= ConsumeThreshold ? true : false;
       
    }
}
