using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchinaceaElixir : NonTargetCard
{
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        return base.CardEffect();
    }
}
