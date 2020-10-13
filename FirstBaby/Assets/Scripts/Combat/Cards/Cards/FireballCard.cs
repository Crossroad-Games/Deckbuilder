using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCard : TargetCard
{
    private int BaseDamage = 20;// Damage value that will be applied to the Enemy
    private int AddValue = 0, SubtractValue = 0;// Values that modify the base value
    private float Multiplier = 1, Divider = 1;// Values that multiply or divide the modified base value
    // This effect deals damage to a single enemy
    public override void CardEffect() => TargetEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
    public override void Start()
    {
        base.Start();
        CardCD = 2;// This card's CD
    }
}
