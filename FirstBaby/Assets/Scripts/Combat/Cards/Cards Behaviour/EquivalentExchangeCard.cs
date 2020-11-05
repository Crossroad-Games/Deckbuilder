using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquivalentExchangeCard : ConcoctCardEffectNonTarget
{
    private bool endDiscardDraw = false;

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
        for(int i = cardsConcocted.Count-1; i>=0; i--)
        {
            playerHand.SendCard(cardsConcocted[i].gameObject, Player.CdPile);
        }
        playerHand.DrawCards(cardsConcocted.Count);
        effectFinished = true;
    }
}
