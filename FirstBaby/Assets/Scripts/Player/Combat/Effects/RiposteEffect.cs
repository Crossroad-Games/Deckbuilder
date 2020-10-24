using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiposteEffect : SingleUseEffect
{
    public override void Effect(EnemyClass attackingEnemy, int Damage)
    {
        attackingEnemy.ProcessDamage((Damage/2 + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        base.Effect(attackingEnemy, Damage);
    }
}
