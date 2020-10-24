using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riposte : NonTargetCard
{
    public override void CardEffect()
    {
        RiposteEffect riposteToAdd  = Player.gameObject.AddComponent<RiposteEffect>() as RiposteEffect; 
        riposteToAdd.InitializeRiposte(0, 0, 1, 1);
    }

    public override void Start()
    {
        base.Start();
    }


    
}
