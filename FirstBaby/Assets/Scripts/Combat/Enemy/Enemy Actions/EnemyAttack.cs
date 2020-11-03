using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyAction
{
    public override IEnumerator Effect()
    {
        // Deal damage to the player
        var Damage = CalculateAction(myInfo.BaseDamage);
        Player.ProcessDamage(myClass, Damage);// Apply damage to the player
        while (!ActionDone)
        {
            yield return new WaitForSeconds(1f);
            ActionDone = true;
            Debug.LogWarning("Needs to update this part to get ActionDone from animator and change the delay");
        }
    }
}   
