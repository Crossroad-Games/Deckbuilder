using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GentleBreezeOverflow : OverflowKeyword
{
    private Hand myHand;// Hand class will be accessed to draw cards when this overflows
    [SerializeField] public int DrawAmount = 3;// Draw X cards when overflowing
    protected override void Awake()
    {
        base.Awake();
        myHand = combatPlayer.GetComponent<Hand>();// Reference to the Hand is set
    }
    public override void OverflowEffect()
    {
        Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
        GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/GenericDefenseKeywordEffect/GenericDefenseKeywordEffect"), new Vector3(-1.92f, 0.25f, -0.23f), Quaternion.identity) as GameObject;
        visualEffect.GetComponent<GenericDefenseKeywordEffect>().virtualCard = this.myCard;

        //Instantiate card UI
        GameObject canvas = GameObject.Find("Canvas");
        GameObject cardUI = Instantiate(Resources.Load("UI/Cards UI/" + myCard.cardInfo.ID), keywordCardUIPosition, Quaternion.identity, canvas.transform) as GameObject;
        visualEffect.GetComponent<GenericDefenseKeywordEffect>().cardUI = cardUI;
    }

    public override void DealEffect()
    {
        myHand.DrawCards(DrawAmount);// Draw this many cards when overflowing
    }
}
