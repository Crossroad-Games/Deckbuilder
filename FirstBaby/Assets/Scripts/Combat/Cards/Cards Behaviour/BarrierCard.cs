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
    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                BaseShield = 8;// Gain 8 Ward
                break;
            case 1:// One LVL higher than base
                BaseShield = 16;// Gains 16 Ward
                break;
            case 2:// Two LVLs higher than base
                BaseShield = 25;// Gains 25 Ward
                break;
        }
    }
}
