using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agony : EnemyLastingEffect
{
    public override void Effect(EnemyClass attackingEnemy, int Damage)
    {
        base.Effect(attackingEnemy, Damage);
    }

    public override void Effect()
    {
        base.Effect();
    }

    public override void InitializeEffect(int BaseValue, int AddValue, int SubtractValue, float Multiplier, float Divider, int turnCounter)
    {
        base.InitializeEffect(BaseValue, AddValue, SubtractValue, Multiplier, Divider, turnCounter);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TurnManager.PlayerTurnStart -= Effect;
    }

    protected override void Start()
    {
        base.Start();
        TurnManager.PlayerTurnStart += Effect;
    }

    protected override void Update()
    {
        base.Update();
    }
}
