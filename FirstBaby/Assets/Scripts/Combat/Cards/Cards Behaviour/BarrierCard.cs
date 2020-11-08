using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCard : NonTargetCard
{
    // This effect creates a shield that will protect the player by this amount
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
}
