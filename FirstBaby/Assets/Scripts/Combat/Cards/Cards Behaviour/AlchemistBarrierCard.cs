using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistBarrierCard : ConcoctCardDefense
{
    public override void BringConcoctInfo(List<PhysicalCard> cardsConcocted) //Get the info from concocted cards
    {
        foreach (PhysicalCard card in cardsConcocted)
        {
            BaseShield += card.BaseShield + Mathf.CeilToInt(0.5f * card.BaseShield); // Add the base shield of the Alchemist barrier for each concocted cards with their own base shields
            Debug.Log(BaseShield);
        }
    }

    public override void GainShield_Health(List<PhysicalCard> cardsConcocted) //Gain shield based on BringConcoctInfo method
    {
        Player.GainShield((BaseShield + AddValue - SubtractValue) * ((int)(Multiplier / Divider))); //GainShield method called on player
        effectFinished = true;
    }

    public override void DoEffects(List<PhysicalCard> cardsConcocted) // Implement if we want this card to call the concocted cards effects
    {
        throw new System.NotImplementedException();
    }
}
