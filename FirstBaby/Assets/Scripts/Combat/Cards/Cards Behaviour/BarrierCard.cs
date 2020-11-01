using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCard : NonTargetCard
{
    public override void Start()
    {
        base.Start();
        BaseShield = 8; // Shield value that will be subtracted and depleted from the enemy's attack
    }

    // This effect creates a shield that will protect the player by this amount
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
}
