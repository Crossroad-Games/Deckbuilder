using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiptideOverflow : OverflowKeyword
{
    [SerializeField] public int ExtraDamage = 5;
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
        myCard.PhysicalCardBehaviour.AddValue += ExtraDamage;// Increases this card's damage
    }
}
