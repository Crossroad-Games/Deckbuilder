using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : EnemyClass
{
   
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void CombatLogic()
    {
        ActionList["Enemy Attack"].Effect();// Use this action
    }

}
