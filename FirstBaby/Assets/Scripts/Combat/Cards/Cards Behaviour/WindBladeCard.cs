using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeCard : TargetCard
{
    public override void Start()
    {
        base.Start();
        BaseDamage = 15;
    }
    // This effect deals damage to a single enemy
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
}
