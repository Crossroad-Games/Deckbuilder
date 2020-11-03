using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncapacitatedEffect : EnemyLastingEffect
{
    protected override void Start()
    {
        base.Start();
        myClass.Incapacitated = true;// Enemy can't act
        myClass.thisEnemyStartTurn += Effect;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        myClass.Incapacitated = false;// Enemy can act
        myClass.thisEnemyStartTurn -= Effect;
    }
}
