using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaintedSoul : EnemyClass
{
    public override void EnemyIntention()
    {
        IntendedActions.Clear();
        if (Player.myData.PlayerShield <= 0)// If the player has no shield
            IntendedActions.Add(ActionList["Precise Attack"]);// Deal a lot of damage
        else if ((myData.Position == 0 && EnemyManager.CombatEnemies.Count > 1) || (Player.myData.PlayerShield >= 0 && EnemyManager.CombatEnemies.Count == 1) || RandomValue <= .2)
            IntendedActions.Add(ActionList["Leech Shield"]);// Steal some shield
        else
            IntendedActions.Add(ActionList["Enemy Attack"]);// Regular attack
    }
}
