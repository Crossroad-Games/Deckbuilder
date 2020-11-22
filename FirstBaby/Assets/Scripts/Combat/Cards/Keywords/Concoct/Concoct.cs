using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CardPorpuse { Attack, Defense, Effect, Any }
public class Concoct : CardExtension
{
    #region References
    private GameObject concoctUI;
    #endregion
    public bool isConcocting = false; 
    public CardPorpuse concoctPorpuse;
    public List<PhysicalCard> cardsToConcoct = new List<PhysicalCard>();
    [SerializeField] Vector3 concoctCardPosition;



    protected override void Awake()
    {
        base.Awake();
        Keyword = "Concoct";
        myCard = GetComponent<PhysicalCard>();
        combatPlayer = FindObjectOfType<CombatPlayer>();
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();
    }

    protected void Start()
    {
        concoctUI = combatManager.concoctUI;
    }

    protected void Update()
    {
        if (isConcocting)
        {
            DoConcoct(); //Execute the Concoct, basically the concoct selection
        }
    }

    public void DoConcoct()
    {
        transform.position = Vector3.Lerp(transform.position, concoctCardPosition, myCard.CombatProperties.cardDrawingSpeed * Time.deltaTime); //Move the concoct card to the predetermined position
        if (Input.GetMouseButtonDown(0)) // concoct selection (mouse pressed)
        {
            if (combatPlayer.hitInfo.collider != null && combatPlayer.isHoveringCard) //If mouse is over a card when it is pressed
            {
                PhysicalCard card = combatPlayer.hitInfo.collider.gameObject.GetComponent<PhysicalCard>(); // auxiliar card variable pointing to the pressed card
                if (concoctPorpuse != CardPorpuse.Any) 
                {
                    if (!card.concocted && card.cardPorpuse == concoctPorpuse && !card.isConcoct)  // if card hasn't been concocted yet, if card isn't the one concocting and if the card porpuse matches the concoct porpuse
                    {
                        if (cardsToConcoct.Count == 0)
                        {
                            cardsToConcoct.Add(card); // add pressed card to concocted cards list
                            card.concocted = true; // turn the card concocted boolean true
                        }
                        else
                        {
                            if (card.GetType() == cardsToConcoct[0].GetType()) // make sure you can only concoct the same type of cards at the same time
                            {
                                cardsToConcoct.Add(card);
                                card.concocted = true;
                            }
                        }
                    }
                    else // unconcoct a card
                    {
                        cardsToConcoct.Remove(card);
                        card.concocted = false;
                        Debug.Log(card.highlightPreviousHeight);
                    }
                }
                else if(concoctPorpuse == CardPorpuse.Any && card != this.GetComponent<PhysicalCard>())
                {
                    if (!card.concocted)
                    {
                        if (cardsToConcoct.Count == 0)
                        {
                            Debug.Log("cardConcocted");
                            cardsToConcoct.Add(card);
                            card.concocted = true;
                            Debug.Log(card.highlightPreviousHeight);
                        }
                        else
                        {
                            Debug.Log("há cartas no concoctlist");
                            cardsToConcoct.Add(card);
                            card.concocted = true;
                            Debug.Log(card.highlightPreviousHeight);
                        }
                    }
                    else
                    {
                        cardsToConcoct.Remove(card);
                        card.concocted = false;
                        Debug.Log(card.highlightPreviousHeight);
                    }
                }
            }

        }
    }

    public void ConfirmConcoct()
    {
        if (cardsToConcoct.Count > 0)
        {
            switch (concoctPorpuse)
            {
                case CardPorpuse.Attack:
                    ConcoctCardAttack myConcoctAttackCard = myCard as ConcoctCardAttack;
                    if (myConcoctAttackCard != null)
                    {
                        myConcoctAttackCard.BringConcoctInfo(cardsToConcoct);
                        //TODO: Call animation here, and make an animation event that will call the DealDamage function
                        Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
                        GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/Test2/Test2"), playerSpriteTransform.position, Quaternion.identity) as GameObject;
                        visualEffect.GetComponent<GenericAttackEffect>().targetTransform = myConcoctAttackCard.TargetEnemy.transform;
                        visualEffect.GetComponent<GenericAttackEffect>().card = myConcoctAttackCard;
                        visualEffect.GetComponent<GenericAttackEffect>().dealEffect = false;
                        FinishConcoct();
                    }
                    break;
                case CardPorpuse.Defense:
                    ConcoctCardDefense myConcoctDefenseCard = myCard as ConcoctCardDefense;
                    if (myConcoctDefenseCard != null)
                    {
                        myConcoctDefenseCard.BringConcoctInfo(cardsToConcoct);
                        //TODO: Call animation here, and make an animation event that will call the GainShield/health function
                        Transform playerSpriteTransform = GameObject.Find("Player_Sprite").GetComponent<Transform>();
                        GameObject visualEffect = Instantiate(Resources.Load("Visual Effects/GenericDefense/GenericDefense"), new Vector3(-1.92f, 0.25f, -0.23f), Quaternion.identity) as GameObject;
                        visualEffect.GetComponent<GenericDefenseEffect>().card = myConcoctDefenseCard;
                        visualEffect.GetComponent<GenericDefenseEffect>().dealEffect = true;
                        FinishConcoct();
                    }
                    break;
                case CardPorpuse.Effect:
                    ConcoctCardEffectNonTarget myConcoctCardEffectNonTarget = myCard as ConcoctCardEffectNonTarget;
                    ConcoctCardEffectTarget myConcoctCardEffectTarget = myCard as ConcoctCardEffectTarget;
                    if (myConcoctCardEffectNonTarget != null) // NonTarget concoct card
                    {
                        myConcoctCardEffectNonTarget.BringConcoctInfo(cardsToConcoct);
                        myConcoctCardEffectNonTarget.DoEffect(cardsToConcoct);
                        FinishConcoct();
                    }
                    else if (myConcoctCardEffectTarget != null)// Target concoct card
                    {
                        myConcoctCardEffectTarget.BringConcoctInfo(cardsToConcoct);
                        myConcoctCardEffectTarget.DoEffect(cardsToConcoct);
                        FinishConcoct();
                    }
                    break;
                case CardPorpuse.Any:
                    ConcoctCardEffectNonTarget myConcoctAnyCardNonTarget = myCard as ConcoctCardEffectNonTarget;
                    ConcoctCardEffectTarget myConcoctAnyCardTarget = myCard as ConcoctCardEffectTarget;
                    if (myConcoctAnyCardNonTarget != null) // NonTarget concoct card
                    {
                        myConcoctAnyCardNonTarget.BringConcoctInfo(cardsToConcoct);
                        myConcoctAnyCardNonTarget.DoEffect(cardsToConcoct);
                        FinishConcoct();
                    }
                    else if (myConcoctAnyCardTarget != null)// Target concoct card
                    {
                        myConcoctAnyCardTarget.BringConcoctInfo(cardsToConcoct);
                        myConcoctAnyCardTarget.DoEffect(cardsToConcoct);
                        FinishConcoct();
                    }
                    break;
            }
        }
        else
        {
            myCard.SetEffectFinishedTrue(); //Utility to set the effectFinished variable to true, although this is being set directely by the cards
            FinishConcoct();
        }
    }

    public void CancelConcoct() //Called when canceled concoct
    {
        combatManager.confirmConcoctButton.onClick.RemoveAllListeners(); // reset the buttons so can subscribe another card's method
        combatManager.cancelConcoctButton.onClick.RemoveAllListeners();
        Debug.Log("deu unsubscribe");
        isConcocting = false;
        concoctUI.SetActive(false); // disable the concoctUI
        myCard.selectable = true; // Make the card selectable again
        myCard.followCardPositionToFollow = true; //Card restart following it's target in hand
        if (combatPlayer.OnCardUnselected != null)
        {
            combatPlayer.OnCardUnselected(myCard.gameObject);//Call CardUnselected event
            foreach (PhysicalCard card in cardsToConcoct)
            {
                card.concocted = false;
                combatPlayer.OnCardUnselected(card.gameObject); // This is called because the concocted card is also highlighted, and the Unselected event calls unhighlight
            }
        }
        foreach (PhysicalCard card in myCard.playerHand.physicalCardsInHand)
        {
            if (card != myCard)
                card.selectable = true;
        }
        cardsToConcoct.Clear();
        switch (concoctPorpuse)
        {
            case CardPorpuse.Attack:
                ConcoctCardAttack myConcoctAttackCard = myCard as ConcoctCardAttack;
                if (myConcoctAttackCard != null)
                {
                    myConcoctAttackCard.canceledConcoct = true;
                }
                break;
                //TODO: ConcoctDefenseCard canceled = true
                //TODO: ConcoctEffectCard canceled = true
                //TODO: ConcoctHybridCard canceled = true
        }
    }

    public void StartConcoct()
    {
        combatManager.confirmConcoctButton.onClick.AddListener(ConfirmConcoct); //subscribe to the button's onClick event
        combatManager.cancelConcoctButton.onClick.AddListener(CancelConcoct);
        Debug.Log("deu subscribe");
        myCard.followCardPositionToFollow = false; //Card stops following it's target in hand
        myCard.selectable = false;
        isConcocting = true; //Update isConcocting boolean
        foreach(PhysicalCard card in myCard.playerHand.physicalCardsInHand)
        {
            if(card != myCard)
                card.selectable = false;
        }
        //Activate Concoct Confirm/Cancel UI
        concoctUI.SetActive(true);
    }

    public void StartConcoct(EnemyClass targetEnemy)
    {
        combatManager.confirmConcoctButton.onClick.AddListener(ConfirmConcoct);
        combatManager.cancelConcoctButton.onClick.AddListener(CancelConcoct);
        Debug.Log("deu subscribe");
        this.targetEnemy = targetEnemy;
        myCard.followCardPositionToFollow = false; //Card stops following it's target in hand
        myCard.selectable = false;
        isConcocting = true; //Update isConcocting boolean
        foreach (PhysicalCard card in myCard.playerHand.physicalCardsInHand)
        {
            if (card != myCard)
                card.selectable = false;
        }
        //Activate Concoct Confirm/Cancel UI
        concoctUI.SetActive(true);
    }

    public void FinishConcoct() //Called when confirmed concoct
    {
        combatManager.confirmConcoctButton.onClick.RemoveAllListeners();
        combatManager.cancelConcoctButton.onClick.RemoveAllListeners();
        isConcocting = false;
        concoctUI.SetActive(false);
        foreach(PhysicalCard card in cardsToConcoct)
        {
            myCard.playerHand.SendCard(card.gameObject, myCard.Player.CdPile,true);
        }
        foreach (PhysicalCard card in myCard.playerHand.physicalCardsInHand)
        {
            if (card != myCard)
                card.selectable = true;
        }
        cardsToConcoct.Clear();
    }

    public override void ExtensionEffect()
    {
        
    }
}
