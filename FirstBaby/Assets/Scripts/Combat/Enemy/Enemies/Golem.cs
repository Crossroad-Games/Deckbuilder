using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : EnemyClass
{
    [SerializeField]private int DisruptiveCD = 5;// How many turns the golem will wait before using Disruptive Blow again
    [SerializeField] private int CurrentDisruptiveCD = 0;// Current Disruptive value
    [SerializeField] private bool Cocooned=false;
    public override void EnemyIntention()
    {
        IntendedActions.Clear();
        if (!Player.Disrupted && CurrentDisruptiveCD <= 0)// If the player is not disrupted
            IntendedActions.Add(ActionList["Disruptive Blow"]);// Disrupt them and remove a % of their shield
        else if (RandomValue <= .5)
            IntendedActions.Add(ActionList["Enemy Attack"]);// Attack the player
        else if (RandomValue <= .75)
            IntendedActions.Add(ActionList["Protection"]);// Protect itself
        else if (RandomValue <= 1)
            IntendedActions.Add(ActionList["Cocoon"]);// Incapacitates itself and gain defense
    }
    public override void ActionPhase()
    {
        if (!Incapacitated && Cocooned)// When this enemy is no longer incapacitated
        {
            ActionList["Enemy Attack"].Multiplier += 1;// Add one to the damage multiplier
            Cocooned = false;// No longer cocooned
        }
        base.ActionPhase();
        foreach (EnemyAction Action in IntendedActions)// Go through all the actions the enemy intends to perform
            if (Action != null)// Check if its null
            {
                if (Action.ActionName == "Disruptive Blow")// If this enemy used Disruptive Blow this turn
                    CurrentDisruptiveCD = DisruptiveCD;// Apply CD
                if(Action.ActionName=="Cocoon")
                    Cocooned = true;
            }
    }
    public override void EndTurn()
    {
        CurrentDisruptiveCD = CurrentDisruptiveCD - 1 <= 0 ? 0 : CurrentDisruptiveCD - 1;// CD countdown
        base.EndTurn();
    }
}
