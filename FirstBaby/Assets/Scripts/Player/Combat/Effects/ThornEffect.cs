using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornEffect : LastingEffect
{
    protected override void Awake()
    {
        base.Awake();
        player.OnPlayerProcessDamage += Effect;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        player.OnPlayerProcessDamage -= Effect;
    }

    public override void Effect(EnemyClass attackingEnemy, int Damage)
    {
        attackingEnemy.ProcessDamage((int)(BaseValue + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        base.Effect(attackingEnemy, Damage);
    }
}
