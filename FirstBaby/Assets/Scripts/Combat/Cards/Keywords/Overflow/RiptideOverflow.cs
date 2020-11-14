using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiptideOverflow : OverflowKeyword
{
    [SerializeField] public int ExtraDamage = 5;
    public override void OverflowEffect()
    {
        myCard.PhysicalCardBehaviour.AddValue += ExtraDamage;// Increases this card's damage
    }
}
