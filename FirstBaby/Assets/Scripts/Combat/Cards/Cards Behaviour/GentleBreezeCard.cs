﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GentleBreezeCard : NonTargetCard
{
    [SerializeField] private GentleBreezeOverflow myOverflow;// Reference to the overflow will be used on the Levelrank method to determine how many cards will be drawn
    [SerializeField] private HastenKeyword myHasten;// Reference to the Hasten keyword will be used on the LevelRanks method to determine how strong will the hasten effect be

    protected override void Awake()
    {
        myOverflow = GetComponent<GentleBreezeOverflow>();
        myHasten = GetComponent<HastenKeyword>();
        base.Awake();
    }

    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
    public override void LevelRanks()
    {
        switch (CardLevel)
        {
            case 0:// Starting Level, regular values
                myOverflow.DrawAmount = 2;// Draw X cards
                myHasten.HastenAmount = 3;// Hasten X
                break;
            case 1:// One LVL higher than base
                myOverflow.DrawAmount = 2;// Draw X cards
                myHasten.HastenAmount = 5;// Hasten X
                thisVirtualCard.CardName.text += "+";
                break;
            case 2:// Two LVLs higher than base
                myOverflow.DrawAmount = 3;// Draw X cards
                myHasten.HastenAmount = 6;// Hasten X
                thisVirtualCard.CardName.text += "++";
                break;
        }
    }

    protected override void UpdateCardText()
    {
        thisVirtualCard.CardText.text = $"Hasten {myHasten.HastenAmount}\nOverflow:\nDraw {myOverflow.DrawAmount} cards";
    }
}
