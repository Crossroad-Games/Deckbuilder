using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riposte : NonTargetCard
{
    public override IEnumerator CardEffect()
    {
        RiposteEffect riposteToAdd  = Player.gameObject.AddComponent<RiposteEffect>() as RiposteEffect; 
        riposteToAdd.InitializeRiposte(0, 0, 1, 1);
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }

    public override void Start()
    {
        base.Start();
    }
    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                BaseShield = 0;
                break;
            case 1:// One LVL higher than base
                BaseShield = 0;
                break;
            case 2:// Two LVLs higher than base
                BaseShield = 0;
                break;
        }
    }


}
