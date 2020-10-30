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

    public override void DealDamage()
    {
        TargetEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        effectFinished = true;
    }
}
