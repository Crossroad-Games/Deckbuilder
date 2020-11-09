using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodexOfKnowledge : NonTargetCard
{
    public override IEnumerator CardEffect()
    {
        playerHand.DrawCards(2);
        effectFinished = true;
        return base.CardEffect();
    }
}
