using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseUpEffect : EnemyLastingEffect
{
    protected override void Awake()
    {
        EffectLabel = "Defense Up";
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        myClass.myData.EnemyDefense += turnCounter;
        myClass.thisEnemyStartTurn += Effect;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        myClass.myData.EnemyDefense -= turnCounter;
        myClass.thisEnemyStartTurn -= Effect;
    }
    public override void AddStacks(int Amount)
    {
        base.AddStacks(Amount);
        myClass.myData.EnemyDefense += Amount;
    }
    public override void Effect()
    {
        myClass.myData.EnemyDefense--;// Reduce one per turn
        base.Effect();
    }
    public override void Effect(EnemyClass attackingEnemy, int Damage)
    {
        myClass.myData.EnemyDefense--;// Reduce one per turn
        base.Effect(attackingEnemy, Damage);
    }
}
