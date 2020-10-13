using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCard : Card
{
    private int BaseShield=10;// Shield value that will be subtracted and depleted from the enemy's attack
    private int AddValue=0, SubtractValue=0;// Values that modify the base value
    private float Multiplier=1, Divider=1;// Values that multiply or divide the modified base value
    // This effect creates a shield that will protect the player by this amount
    public override void CardEffect() => Player.GainShield((BaseShield + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
    public override void Start()
    {
        base.Start();
        CardCD = 1;// This card's CD
    }
}
