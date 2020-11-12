using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncapacitatedEffect : EnemyLastingEffect
{
    protected override void Awake()
    {
        EffectLabel = "Incapacitated";
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        myClass.Incapacitated = true;// Enemy can't act
        myClass.thisEnemyEndTurn += Effect;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        myClass.Incapacitated = false;// Enemy can act
        myClass.thisEnemyEndTurn -= Effect;
    }
}
