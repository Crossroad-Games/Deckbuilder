using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyAction
{
    public override void Effect()
    {
        // Deal damage to the player
        var Damage = CalculateAction(myInfo.BaseDamage);
        Player.ProcessDamage(myClass,Damage);// Apply damage to the player
        
    }
    
}   
