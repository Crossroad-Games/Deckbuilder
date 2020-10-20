using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengefulSpirit : EnemyClass
{
    public override void EnemyIntention()
    {
        IntendedActions.Clear();// Clears this enemy's intend action list
        if (((myData.EnemyHP + myData.EnemyShield) <= myData.EnemyMaxHP / 2) && EnemyManager.CombatEnemies.Count > 1)// If at or below 25% HP
            IntendedActions.Add(ActionList["Blood Ritual"]);// Attack for double damage
        else if (Player.myData.PlayerShield == 0)// If the player does not have any shield or 75% from random behaviour
            IntendedActions.Add(ActionList["Precise Attack"]);// Attack for double damage
        else
            IntendedActions.Add(ActionList["Enemy Attack"]);// Attack and a small amount of damage
    }
}
