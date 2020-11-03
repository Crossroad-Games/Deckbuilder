using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

public class LeechShield : EnemyAction
{
    public override IEnumerator Effect()
    {
        var ShieldDamage = CalculateAction(myInfo.BaseDamage);// Calculates the final shield leech
        var ShieldLeeched = (Player.myData.PlayerShield - ShieldDamage) < 0 ? 0 : ShieldDamage;// If there was nothing to leech, then don't gain any shield
        Player.LoseShield(ShieldDamage);// Deal damage to the player's shield
        myClass.GainShield(ShieldLeeched);// Gain the amount of shield stolen from the player
        while (!ActionDone)
        {
            yield return new WaitForSeconds(1f);
            ActionDone = true;
            Debug.LogWarning("Needs to update this part to get ActionDone from animator and change the delay");
        }
    }
}
