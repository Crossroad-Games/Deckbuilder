using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckthornPowder : TargetCard
{
    protected override void Awake()
    {
        base.Awake();
        BaseDamage = 10;
    }

    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        return base.CardEffect();
    }
}
