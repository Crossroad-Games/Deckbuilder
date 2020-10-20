using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protection : EnemyAction
{
    public override void Effect()
    {
        myClass.GainShield(CalculateAction(myInfo.BaseShield));// Gain shield
    }
}
