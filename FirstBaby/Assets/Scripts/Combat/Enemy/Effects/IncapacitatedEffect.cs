using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncapacitatedEffect : EnemyLastingEffect
{
    protected override void Start()
    {
        base.Start();
        myClass.Incapacitated = true;// Enemy can't act
        TurnManager.EnemyStartTurn += Effect;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        myClass.Incapacitated = false;// Enemy can act
        TurnManager.EnemyStartTurn -= Effect;
    }
}
