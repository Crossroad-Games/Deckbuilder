using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Concoct))]
public class AlchemistBladeCard : ConcoctCardAttack
{
    public override void BringConcoctInfo(List<Card> cardsConcocted)
    {
        foreach(Card card in cardsConcocted)
        {
            BaseDamage += card.BaseDamage + Mathf.CeilToInt(0.5f * card.BaseDamage);
            Debug.Log(BaseDamage);
        }
    }

    public override void DealDamage(List<Card> cardsConcocted)
    {
        TargetEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        Debug.Log("alchemist blade deal damage");
        if (doEffects)
            DoEffects(cardsConcocted);
        effectFinished = true;
    }

    public override void DoEffects(List<Card> cardsConcocted)
    {
        Debug.Log("alchemsit blade do effects");
        foreach(Card card in cardsConcocted)
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
