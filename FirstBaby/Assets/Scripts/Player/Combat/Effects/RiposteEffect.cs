using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiposteEffect : SingleUseEffect
{
    protected override void Awake()
    {
        EffectLabel = "Reflect";
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
        attackingEnemy.ProcessDamage((Damage/2 + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        base.Effect(attackingEnemy, Damage);
    }
}
