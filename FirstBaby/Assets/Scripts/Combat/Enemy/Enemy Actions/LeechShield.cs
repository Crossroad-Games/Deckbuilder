using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechShield : EnemyAction
{
    [Header("Action values")]
    [SerializeField] private int BaseDamage = 10;
    void Awake()
    {
        BaseDamage = myInfo.BaseDamage;// Copies the value of the action info
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Effect()
    {
        var ShieldDamage = CalculateAction(BaseDamage);// Calculates the final shield leech
        var ShieldLeeched = (Player.myData.PlayerShield-ShieldDamage) < 0 ? 0 : ShieldDamage;// If there was nothing to leech, then don't gain any shield
        Player.LoseShield(ShieldDamage);// Deal damage to the player's shield
        myClass.GainShield(ShieldLeeched);// Gain the amount of shield stolen from the player
    }
}
