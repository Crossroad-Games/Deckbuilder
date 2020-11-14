using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GentleBreezeOverflow : OverflowKeyword
{
    private Hand myHand;// Hand class will be accessed to draw cards when this overflows
    [SerializeField] public int DrawAmount = 3;// Draw X cards when overflowing
    protected override void Awake()
    {
        base.Awake();
        myHand = combatPlayer.GetComponent<Hand>();// Reference to the Hand is set
    }
    public override void OverflowEffect()
    {
        myHand.DrawCards(DrawAmount);// Draw this many cards when overflowing
    }
}
