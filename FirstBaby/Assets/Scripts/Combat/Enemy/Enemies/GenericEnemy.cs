using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : EnemyClass
{
   
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void EnemyIntention()
    {
        IntendedActions.Clear();
        IntendedActions.Add(ActionList["Enemy Attack"]);// Use this action
    }

}
