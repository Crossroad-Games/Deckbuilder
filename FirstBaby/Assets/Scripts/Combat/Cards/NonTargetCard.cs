using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetCard : PhysicalCard
{

    private void OnEnable()
    {
        this.type = "NonTargetCard";
    }



    public override IEnumerator CardEffect() // This is the field used by the card to describe and execute its action
    {
        yield return StartCoroutine(base.CardEffect());
    }
}
