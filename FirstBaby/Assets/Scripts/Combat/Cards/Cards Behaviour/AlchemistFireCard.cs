using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistFireCard : TargetCard
{

    protected override void Awake()
    {
        base.Awake();
        BaseDamage = 5;
    }
    // This effect deals damage to a single enemy
    public override IEnumerator CardEffect()
    {
        TargetEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
}
