using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    #region Reference
    public CombatPlayer Player;
    private CombatManager combatManager;
    public CardInfo cardInfo;
    public Hand playerHand;
    public EnemyClass TargetEnemy;
    [SerializeField] private CombatProperties combatProperties=null;
    public CombatProperties CombatProperties { get { return combatProperties; } }
    #endregion

    #region Booleans
    public bool selected; //when card is being selected
    public bool concocted = false; //When card is being concocted by another card
    public bool selectable = true; //Determines wether card is selectable or not
    public bool highlighted; // when card is being highlighted
    public float highlightPreviousHeight; // When the card will be highlighted, stores the height of the card of when it wasn't highlighted
    public Quaternion highlightPreviousRotation; // When the card will be highlighted, stores the rotation of the card of when it wasn't highlighted
    public bool hoverEffectsEnabled = true; //wether hover effects are enabled for this card
    public bool beingDrawn; // when card is being drawn
    public bool returningToHand; // when card is returning to hand
    public bool beingHovered; // when card is being hovered by mouse
    protected bool canGotoCDPile = false; // Flag for when card finishes all of it's desired behaviours
    protected bool effectFinished = false; // Flag for when should call the Effect Finished callback
    #endregion

    public string type = "none";
    public CardPorpuse cardPorpuse;

    #region CardValues
    public int BaseDamage = 0;// Damage value that will be applied to the Enemy
    public int BaseShield = 0; //Value of shield to gain
    public int BaseHeal = 0; //Value to heal directly in life
    public int AddValue = 0, SubtractValue = 0;// Values that modify the base value
    public float Multiplier = 1, Divider = 1;// Values that multiply or divide the modified base value
    #endregion

    public bool followCardPositionToFollow; //true if we want card to follow target

    protected virtual void Awake()
    {
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();
    }

    public virtual void Start()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * combatProperties.cardNormalScale;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();
        playerHand = Player.GetComponent<Hand>();
        highlighted = false;
        selectable = true;
        selected = false;
    }
    public virtual IEnumerator ExecuteAction()
    {
        Debug.Log("chamou executeAction coroutine");
        StartCoroutine(CardEffect());// Execute the card's effect
        yield return new WaitUntil(() => canGotoCDPile == true); //Suspends the coroutine execution until the canGoToCDPile flag is set to true
        //Send it to the CD pile
        if (!Player.CombatManager.Won && !Player.CombatManager.Defeated)
            playerHand.SendCard(this.cardInfo, Player.CdPile); //Send cardInfo to CDPile
    }

    public virtual IEnumerator ExecuteAction(EnemyClass targetEnemy)
    {
        this.TargetEnemy = targetEnemy;
        Debug.Log("chamou executeAction coroutine");
        StartCoroutine(CardEffect()); // Execute the card's effect
        yield return new WaitUntil(() => canGotoCDPile == true); //Suspends the coroutine execution until the supplied delegate evaluates to true
        //Send it to the CD pile
        Debug.Log("vai mandar pro cdPile");
        if (!Player.CombatManager.Won && !Player.CombatManager.Defeated)
            playerHand.SendCard(this.cardInfo, Player.CdPile); //Send cardInfo to CDPile
    }

    public virtual void EndCardEffect()
    {
        canGotoCDPile = true;
    }

    public virtual IEnumerator CardEffect()  // This is the field used by the card to describe and execute its action
    {
        yield return new WaitUntil(() => effectFinished == true);
        EndCardEffect();
    }

    
}
