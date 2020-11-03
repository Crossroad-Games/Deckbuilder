using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

public class Protection : EnemyAction
{
    public override IEnumerator Effect()
    {
        myClass.GainShield(CalculateAction(myInfo.BaseShield));// Gain shield
        while (!ActionDone)
        {
            yield return new WaitForSeconds(1f);
            ActionDone = true;
            Debug.LogWarning("Needs to update this part to get ActionDone from animator and change the delay");
        }
    }
}
