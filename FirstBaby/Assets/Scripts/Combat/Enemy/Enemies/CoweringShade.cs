using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoweringShade : EnemyClass
{
    private bool TookDamage;
    public override void EnemyIntention()
    {
        IntendedActions.Clear();
        if (TookDamage || RandomValue <= .2)
            IntendedActions.Add(ActionList["Protection"]);
        else if (RandomValue <= .8)
            IntendedActions.Add(ActionList["Enemy Attack"]);
        else
            IntendedActions.Add(ActionList["Leech Shield"]);
    }
    public override void ProcessDamage(int Damage)
    {
        var CurrentHP = myData.EnemyHP;// Acquire the HP before damage is applied
        base.ProcessDamage(Damage);
        TookDamage = !(CurrentHP == myData.EnemyHP);// If the Current HP is the same after taking this attack, this enemy didn't take damage
    }
    public override void EndTurn()
    {
        base.EndTurn();
        TookDamage = false;// Turn ended, reset this boolean
    }
}
