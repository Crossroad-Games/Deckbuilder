using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEfect : LastingEffect
{
    private float RetainIncreasePerStack = 0.1f; //How much shield retain increase per stack

    protected override void Awake()
    {
        base.Awake();
        turnCounter = 0;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        player.ShieldDecay += turnCounter * RetainIncreasePerStack;// Removes all remaining stacks when this effect is removed
        TurnManager.PlayerTurnStart -= Effect;
    }
    public override void InitializeEffect(int BaseValue, int AddValue, int SubtractValue, float Multiplier, float Divider, int turnCounter)
    {
        base.InitializeEffect(BaseValue, AddValue, SubtractValue, Multiplier, Divider, turnCounter);
        player.ShieldDecay -= turnCounter * RetainIncreasePerStack;// Increases the player's shield decay per stack
        TurnManager.PlayerTurnStart += Effect;// Subscribes the effect
    }
    public override void AddStacks(int StacksToAdd)
    {
        base.AddStacks(StacksToAdd);
        player.ShieldDecay -= StacksToAdd * RetainIncreasePerStack;// Increases the player's shield decay per stack
    }
    public override void Effect()
    {
        player.ShieldDecay += RetainIncreasePerStack;// Removes one stack
        base.Effect();
    }
    public override void Effect(EnemyClass attackingEnemy, int Damage)
    {
        player.ShieldDecay += RetainIncreasePerStack;// Removes one stack
        base.Effect(attackingEnemy, Damage);
    }
}
