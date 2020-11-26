using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquivalentExchangeCard : ConcoctCardEffectNonTarget
{
    private bool endDiscardDraw = false;
    [SerializeField] private int ExtraDraw = 0;
    public override void Start()
    {
        endDiscardDraw = false;
        base.Start();
    }

    public override void BringConcoctInfo(List<PhysicalCard> cardsConcocted)
    {
        
    }

    public override void DoEffect(List<PhysicalCard> cardsConcocted)
    {
        playerHand.DrawCards(cardsConcocted.Count+ExtraDraw);
        effectFinished = true;
    }
    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                ExtraDraw = 0;
                break;
            case 1:// One LVL higher than base
                ExtraDraw = 1;
                thisVirtualCard.CardName.text += "+";
                break;
            case 2:// Two LVLs higher than base
                ExtraDraw = 2;
                thisVirtualCard.CardName.text += "++";
                break;
        }
    }

    protected override void UpdateCardText()
    {
        if (ExtraDraw == 0)
            thisVirtualCard.CardText.text = $"Apply X CD:\nDraw X cards";
        else
            thisVirtualCard.CardText.text = $"Apply X CD:\nDraw X+{ExtraDraw} cards";
    }
}
