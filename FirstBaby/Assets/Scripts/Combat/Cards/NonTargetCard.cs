using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetCard : Card
{

    private void OnEnable()
    {
        this.type = "NonTargetCard";
    }



    public override void CardEffect(){ }// This is the field used by the card to describe and execute its action
}
