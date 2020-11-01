using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistBarrierCard : ConcoctCardDefense
{
    public override void BringConcoctInfo(List<Card> cardsConcocted)
    {
        foreach (Card card in cardsConcocted)
        {
            BaseShield += card.BaseShield + Mathf.CeilToInt(0.5f * card.BaseShield);
            Debug.Log(BaseShield);
        }
    }

    public override void GainShield_Health(List<Card> cardsConcocted)
    {
        Player.GainShield((BaseShield + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        effectFinished = true;
    }

    public override void DoEffects(List<Card> cardsConcocted)
    {
        throw new System.NotImplementedException();
    }
}
