using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Concoct))]
public class AlchemistBladeCard : ConcoctCardAttack
{
    [SerializeField] private float ConcoctMultiplier = .5f;
    public override void BringConcoctInfo(List<PhysicalCard> cardsConcocted) // Bring info from the concocted cards
    {
        foreach(PhysicalCard card in cardsConcocted)
        {
            BaseDamage += card.BaseDamage + Mathf.CeilToInt(ConcoctMultiplier * card.BaseDamage); //Update this card's Attack based on the concocted cards' baseDamages
            Debug.Log(BaseDamage);
        }
    }

    public override IEnumerator DealDamage(List<PhysicalCard> cardsConcocted) 
    {
        TargetEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider))); //Call the damange after got the info with BringConcoctInfo()
        Debug.Log("alchemist blade deal damage");
        if (doEffects)
            DoEffects(cardsConcocted); //if should do the card's effects, then execute them
        effectFinished = true;
        Debug.Log("effect finished true");
        yield return StartCoroutine(base.DealDamage(cardsConcocted));
    }

    public override IEnumerator DoEffects(List<PhysicalCard> cardsConcocted)
    {
        Debug.Log("alchemsit blade do effects");
        foreach(PhysicalCard card in cardsConcocted) //this calls the CardEffect() of every concocted card that has the doEffectWhenConcocted flag set to true
        {
            if(card.doEffectWhenConcocted)
            {
                if (card.type == "TargetCard" || card.type == "ConcoctCardAttack" || card.type == "ConcoctCardEffectTarget")
                    card.TargetEnemy = TargetEnemy;
                StartCoroutine(card.CardEffect());
            }
        }
        yield return StartCoroutine(base.DoEffects(cardsConcocted));
    }
    public override void LevelRanks()
    {
        switch (CardLevel)
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
        thisVirtualCard.CardText.text = "Transmute Offensive Energy";
    }
}
