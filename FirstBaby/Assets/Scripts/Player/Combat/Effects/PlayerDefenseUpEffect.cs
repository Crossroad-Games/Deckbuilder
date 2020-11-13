using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseUpEffect : LastingEffect
{
    protected override void Awake()
    {
        EffectLabel = "Defense Up";
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        player.ArmorUpdate(turnCounter);// Increases the armor by the amount of stacks, which coincides with the duration of the effect
        TurnManager.PlayerTurnStart += Effect;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        player.ArmorUpdate(-turnCounter);// Decreases the armor by the amount of stacks, which coincides with the duration of the effect
        TurnManager.PlayerTurnStart -= Effect;
    }
    public override void AddStacks(int Amount)
    {
        base.AddStacks(Amount);
        player.ArmorUpdate(Amount);// Increases the armor by the amount of stacks, which coincides with the duration of the effect
    }
    public override void Effect()
    {
        player.ArmorUpdate(-1);// Increases the armor by the amount of stacks, which coincides with the duration of the effect
        base.Effect();
    }
    public override void Effect(EnemyClass attackingEnemy, int Damage)
    {
        player.ArmorUpdate(-1);// Increases the armor by the amount of stacks, which coincides with the duration of the effect
        base.Effect(attackingEnemy, Damage);
    }
}
