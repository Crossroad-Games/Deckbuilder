using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianSpirit : EnemyClass
{
   
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void EnemyIntention()
    {
        IntendedActions.Clear();
        if(myData.EnemyShield==0 || RandomValue<=.7)//  If this enemy doesn't have any shield or 70% chance random action
            IntendedActions.Add(ActionList["Protection"]);// Use this action
        else// If the enemy has shield there is a 30% chance of a random action
            IntendedActions.Add(ActionList["Enemy Attack"]);// Use this action
    }

}
