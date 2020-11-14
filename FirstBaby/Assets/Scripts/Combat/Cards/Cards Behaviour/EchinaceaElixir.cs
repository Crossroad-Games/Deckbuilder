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
    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                BaseShield = 16;
                break;
            case 1:// One LVL higher than base
                BaseShield = 30;
                break;
            case 2:// Two LVLs higher than base
                BaseShield = 45;
                break;
        }
    }
}
