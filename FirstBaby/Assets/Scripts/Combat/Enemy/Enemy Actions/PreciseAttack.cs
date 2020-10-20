using System.Collections;
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
    public override void Effect()
    {
        var Damage = CalculateAction(myInfo.BaseDamage);// Calculates the final damage
        
        Player.ProcessDamage(myClass,Damage);// Apply damage to the player
    }
    public override void ShowValue()
    {
        if (Player.myData.PlayerShield == 0)// If the player doesnt have any shield
            Multiplier = 2;// Double its damage
        base.ShowValue();
    }
}
