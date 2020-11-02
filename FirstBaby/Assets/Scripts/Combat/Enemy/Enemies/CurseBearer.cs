using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseBearer : EnemyClass
{
    [SerializeField]private int BreathCD=5, CurrentBreathCD=0;
    private bool Cocooned;// If this enemy is cocooned
    private int CocoonDuration;
    public override void EnemyIntention()
    {
        IntendedActions.Clear();
        if (RandomValue <= .2 && CurrentBreathCD <= 0)
            IntendedActions.Add(ActionList["Decaying Breath"]);
        else if (RandomValue <= .7)
            IntendedActions.Add(ActionList["Enemy Attack"]);
        else if (RandomValue<=1)
            IntendedActions.Add(ActionList["Cocoon"]);
    }
    public override void ActionPhase()
    {
        base.ActionPhase();
        foreach (EnemyAction Action in IntendedActions)// Go through all the actions the enemy intends to perform
            if (Action != null)// Check if its null
            {
                if (Action.ActionName == "Decaying Breath")// If this enemy used Disruptive Blow this turn
                    CurrentBreathCD = BreathCD;// Apply CD
                if (Action.ActionName == "Cocoon")// If this enemy used Cocoon this turn
                {
                    CocoonDuration = GetComponent<Cocoon>().TurnCount;// Get the duration of the cocoon
                    Cocooned = true;
                }
            }
    }
    public override void StartTurn()
    {
        CurrentBreathCD = CurrentBreathCD >= 1 ? CurrentBreathCD-1 : 0;// Update the CD
        base.StartTurn();
    }
    public override void EndTurn()
    {
        RandomValue = Random.value;// Generates a random value every end of turn
        thisEnemyEndTurn?.Invoke();// Invoke all methods subscribed to this event
        CocoonDuration = CocoonDuration >= 1 ? CocoonDuration - 1 : 0;// Countdown
        if (CocoonDuration <= 0 && Cocooned)// When the cocoon duration is over
        {
            if (!Incapacitated)// Loses the effect if incapacitated
                ActionList["Miasma"].Effect();// Use this action
            Cocooned = false;// No longer cocooned
        }
        EnemyManager.EndEnemyTurn();
    }
}
