using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckthornPowder : TargetCard
{
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        return base.CardEffect();
    }
}
