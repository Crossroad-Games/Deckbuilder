using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLanceCard : TargetCard
{
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
}
