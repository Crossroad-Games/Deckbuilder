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
    [SerializeField] protected int CardCD;// This card's CD
    [SerializeField] protected int CurrentCD;// This card's current CD which will decrement every turn until it reached 0
    // Once it reaches 0, it will be moved from the CD pile to the Deck and shuffle it during the beginning of the player's turn
    public bool Selected;

    public bool followTarget; //true if we want card to follow target


    public virtual void Start()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * combatProperties.cardNormalScale;
    }
    public void ExecuteAction()
    {
        CardEffect();// Execute the card's effect
        CurrentCD = CardCD;// Apply this card's CD
        //Send it to the CD pile
    }
    public abstract void CardEffect();// This is the field used by the card to describe and execute its action

    
}
