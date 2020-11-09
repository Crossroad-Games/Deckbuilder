using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : LastingEffect
{
    [SerializeField] private int BurnDamage=5;// Burn Damage per stack 
    protected override void Awake()
    {
        EffectLabel = "Burn";
        base.Awake();
        TurnManager.PlayerTurnEnd += Effect;// Apply damage at the end of the player's turn
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        TurnManager.PlayerTurnEnd -= Effect;// Apply damage at the end of the player's turn
    }
    public override void Effect()
    {
        player.ProcessDamage(null, BurnDamage * Mathf.CeilToInt(turnCounter / 2f));// Deals half the stacks rounded up times base burn damage
        turnCounter-=Mathf.CeilToInt(turnCounter / 2f);// Removes half, rounded up, stacks of burn
        if (turnCounter <= 0)// If there are no more stacks
            Destroy(this);// Destroy
    }
    public override void Effect(EnemyClass attackingEnemy, int Damage)
    {
        player.ProcessDamage(null, BurnDamage * Mathf.CeilToInt(turnCounter / 2f));// Deals half the stacks rounded up times base burn damage
        turnCounter -= Mathf.CeilToInt(turnCounter / 2f);// Removes half, rounded up, stacks of burn
        if (turnCounter <= 0)// If there are no more stacks
            Destroy(this);// Destroy
    }
}
