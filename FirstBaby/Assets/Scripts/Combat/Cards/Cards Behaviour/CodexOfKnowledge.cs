using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodexOfKnowledge : NonTargetCard
{
    [SerializeField] private int AmountToDraw=2;
    public override IEnumerator CardEffect()
    {
        playerHand.DrawCards(AmountToDraw);
        effectFinished = true;
        return base.CardEffect();
    }
    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                AmountToDraw = 2;
                break;
            case 1:// One LVL higher than base
                AmountToDraw = 3;
                thisVirtualCard.CardName.text += "+";
                break;
            case 2:// Two LVLs higher than base
                AmountToDraw = 4;
                thisVirtualCard.CardName.text += "++";
                break;
        }
    }

    protected override void UpdateCardText()
    {
        thisVirtualCard.CardText.text = $"Draw {AmountToDraw} cards";
    }
}
