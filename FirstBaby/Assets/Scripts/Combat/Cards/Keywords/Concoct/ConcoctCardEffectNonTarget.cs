﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConcoctCardEffectNonTarget : NonTargetCard
{
    public Concoct myConcoct;
    public bool canceledConcoct = false;

    protected override void Awake()
    {
        base.Awake();
        myConcoct = GetComponent<Concoct>();
        isConcoct = true;
    }

    public override void Start()
    {
        base.Start();
        type = "ConcoctCardEffectNonTarget";
    }

    public override IEnumerator CardEffect()
    {
        myConcoct.StartConcoct(TargetEnemy);
        yield return StartCoroutine(base.CardEffect());
    }

    public override IEnumerator ExecuteAction(EnemyClass targetEnemy)
    {
        this.TargetEnemy = targetEnemy;
        Debug.Log("chamou executeAction coroutine");
        if (cardPorpuse == CardPorpuse.Attack)
            DealDamage();
        else if (cardPorpuse == CardPorpuse.Defense)
            GainShield_Health();
        StartCoroutine(CardEffect()); // Execute the card's effect
        yield return new WaitUntil(() => canGotoCDPile == true || canceledConcoct); //Suspends the coroutine execution until the supplied delegate evaluates to true
        if (canGotoCDPile)
        {
            //Send it to the CD pile
            Debug.Log("vai mandar pro cdPile");
            if (!Player.CombatManager.Won && !Player.CombatManager.Defeated)
                playerHand.SendCard(this.gameObject, Player.CdPile); //Send cardInfo to CDPile
        }
        else
        {
            canceledConcoct = false;
            yield break;
        }
    }

    public abstract void BringConcoctInfo(List<PhysicalCard> cardsConcocted);

    public abstract void DoEffect(List<PhysicalCard> cardsConcocted);
}
