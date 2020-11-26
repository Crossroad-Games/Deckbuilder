using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistBarrierCard : ConcoctCardDefense
{
    [SerializeField] private float ConcoctMultiplier=.5f;// Used to determine how strong the concoct effect is
    public override void BringConcoctInfo(List<PhysicalCard> cardsConcocted) //Get the info from concocted cards
    {
        foreach (PhysicalCard card in cardsConcocted)
        {
            BaseShield += card.BaseShield + Mathf.CeilToInt(ConcoctMultiplier * card.BaseShield); // Add the base shield of the Alchemist barrier for each concocted cards with their own base shields
            Debug.Log(BaseShield);
        }
    }

    public override IEnumerator GainShield_Health(List<PhysicalCard> cardsConcocted) //Gain shield based on BringConcoctInfo method
    {
        Player.GainShield((BaseShield + AddValue - SubtractValue) * ((int)(Multiplier / Divider))); //GainShield method called on player
        effectFinished = true;
        yield return StartCoroutine(base.GainShield_Health(cardsConcocted));
    }

    public override IEnumerator DoEffects(List<PhysicalCard> cardsConcocted) // Implement if we want this card to call the concocted cards effects
    {
        yield return StartCoroutine(base.DoEffects(cardsConcocted));
    }

    public override void LevelRanks()
    {
        switch(CardLevel)
        {
            case 0:// Starting Level, regular values
                ConcoctMultiplier = .5f;
                break;
            case 1:// One LVL higher than base
                ConcoctMultiplier = .75f;
                thisVirtualCard.CardName.text += "+";
                break;
            case 2:// Two LVLs higher than base
                ConcoctMultiplier = 1;
                thisVirtualCard.CardName.text += "++";
                break;
        }
    }

    protected override void UpdateCardText()
    {
        thisVirtualCard.CardText.text = "Transmute Defensive Energy";
    }
}
