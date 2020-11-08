using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseUpEffect : LastingEffect
{
    protected override void Start()
    {
        base.Start();
        player.myData.PlayerDefense += turnCounter;
        TurnManager.PlayerTurnStart += Effect;
    }
    protected override void OnDisable()
    {
        player.myData.PlayerDefense -= turnCounter;
        TurnManager.PlayerTurnStart -= Effect;
    }
    public override void AddStacks(int Amount)
    {
        base.AddStacks(Amount);
        player.myData.PlayerDefense += Amount;
    }
    public override void Effect()
    {
        player.myData.PlayerDefense--;// Reduce one per turn
        base.Effect();
    }
    public override void Effect(EnemyClass attackingEnemy, int Damage)
    {
        player.myData.PlayerDefense--;// Reduce one per turn
        base.Effect(attackingEnemy, Damage);
    }
}
