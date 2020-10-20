using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MischievousSpirit : EnemyClass
{
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
    public override void EnemyIntention()
    {
        IntendedActions.Clear();// Clears this enemy's intend action list
        if ((myData.Position == 0 && (EnemyManager.CombatEnemies.Count) >= 3) || RandomValue>.7)// If there are 3 enemies and this is the first one: Steal the player shield
        {
            // If it is not 100% set to leech shield, 30% chance to leech shield
            IntendedActions.Add(ActionList["Leech Shield"]);// Use this action 
        }
        else if (RandomValue <= .7)// If it is not 100% set to leech shield, 70% chance to attack
        {
            IntendedActions.Add(ActionList["Enemy Attack"]);// Use this action 
        }
    }
}
