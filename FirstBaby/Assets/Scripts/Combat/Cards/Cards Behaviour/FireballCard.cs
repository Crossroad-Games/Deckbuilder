using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCard : TargetCard
{ // This effect deals damage to a single enemy
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
}
