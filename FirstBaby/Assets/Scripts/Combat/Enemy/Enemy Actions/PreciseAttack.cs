using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

public class PreciseAttack : EnemyAction
{
    [Header("Action values")]
    [SerializeField] private int BaseDamage = 6;
    void Awake()
    {
        BaseDamage = myInfo.BaseDamage;
    }
    public override void ShowValue()
    {
        if (Player.myData.PlayerShield == 0)// If the player doesnt have any shield
            Multiplier = 2;// Double its damage
        base.ShowValue();
    }

    public override IEnumerator Effect()
    {
        var Damage = CalculateAction(myInfo.BaseDamage);// Calculates the final damage

        Player.ProcessDamage(myClass, Damage);// Apply damage to the player
        while (!ActionDone)
        {
            yield return new WaitForSeconds(1f);
            ActionDone = true;
            Debug.LogWarning("Needs to update this part to get ActionDone from animator and change the delay");
        }
    }
}
