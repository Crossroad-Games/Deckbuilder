using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornEffect : LastingEffect
{
    public override void Effect(EnemyClass attackingEnemy, int Damage)
    {
        attackingEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        base.Effect(attackingEnemy, Damage);
    }
}
