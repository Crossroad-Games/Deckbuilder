using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaintedSoul : EnemyClass
{
    public override void EnemyIntention()
    {
        IntendedActions.Clear();
    }
}
