using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Concoct))]
public class AlchemistBladeCard : ConcoctCardAttack
{
    public override void BringConcoctInfo(List<PhysicalCard> cardsConcocted) // Bring info from the concocted cards
    {
        foreach(PhysicalCard card in cardsConcocted)
        {
            BaseDamage += card.BaseDamage + Mathf.CeilToInt(0.5f * card.BaseDamage); //Update this card's Attack based on the concocted cards' baseDamages
            Debug.Log(BaseDamage);
        }
    }

    public override void DealDamage(List<PhysicalCard> cardsConcocted) 
    {
        TargetEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider))); //Call the damange after got the info with BringConcoctInfo()
        Debug.Log("alchemist blade deal damage");
        if (doEffects)
            DoEffects(cardsConcocted); //if should do the card's effects, then execute them
        effectFinished = true;
    }

    public override void DoEffects(List<PhysicalCard> cardsConcocted)
    {
        Debug.Log("alchemsit blade do effects");
        foreach(PhysicalCard card in cardsConcocted) //this calls the CardEffect() of every concocted card that has the doEffectWhenConcocted flag set to true
        {
            if(card.doEffectWhenConcocted)
            {
                if (card.type == "TargetCard")
                    card.TargetEnemy = TargetEnemy;
                StartCoroutine(card.CardEffect());
            }
        }
    }
}
