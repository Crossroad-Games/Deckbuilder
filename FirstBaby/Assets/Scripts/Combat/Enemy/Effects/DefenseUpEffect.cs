using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseUpEffect : EnemyLastingEffect
{
    protected override void Start()
    {
        base.Start();
        myClass.myData.EnemyDefense += BaseValue;
        myClass.thisEnemyStartTurn += Effect;
    }
    protected override void OnDisable()
    {
        myClass.myData.EnemyDefense -= BaseValue;
        myClass.thisEnemyStartTurn -= Effect;
    }
}
