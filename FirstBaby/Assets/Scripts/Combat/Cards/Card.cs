using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    #region Reference
    public CombatPlayer Player;
    public CardInfo cardInfo;
    public Hand playerHand;
    public EnemyClass TargetEnemy;
    [SerializeField] CombatProperties combatProperties=null;
    #endregion


    public bool selected;
    public bool highlighted;
    public bool beingDrawn; // when card is being drawn
    public bool returningToHand; // when card is returning to hand
    public bool beingHovered; // when card is being hovered by mouse
    public string type = "none";
    [SerializeField] protected int CardCD;// This card's CD
    [SerializeField] protected int CurrentCD;// This card's current CD which will decrement every turn until it reached 0
    // Once it reaches 0, it will be moved from the CD pile to the Deck and shuffle it during the beginning of the player's turn

    public bool followCardPositionToFollow; //true if we want card to follow target


    public virtual void Start()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * combatProperties.cardNormalScale;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();
        highlighted = false;
    }
    public void ExecuteAction()
    {
        CardEffect();// Execute the card's effect
        CurrentCD = CardCD;// Apply this card's CD
        //Send it to the CD pile
    }

    public void ExecuteAction(EnemyClass targetEnemy)
    {
        this.TargetEnemy = targetEnemy;
        CardEffect();// Execute the card's effect
        CurrentCD = CardCD;// Apply this card's CD
        //Send it to the CD pile
    }

    public abstract void CardEffect();// This is the field used by the card to describe and execute its action

    
}
