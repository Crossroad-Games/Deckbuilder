using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class Hand : CardPile
{

    #region Fields and Properties
    [SerializeField] private Transform handAnchor=null; //Pivot for card positions in hand
    
    public Transform HandAnchor //Getter
    {
        get
        {
            return handAnchor;
        }
    }
    [SerializeField] private Transform cardDrawPosition=null; //Point where cards are created when drawn
    public Transform CardDrawPosition  //Getter
    {
        get
        {
            return cardDrawPosition;
        }
    }
    public Dictionary<Card, CardPositionToFollow> cardPositionsToFollow = new Dictionary<Card, CardPositionToFollow>();    //Entity that cards follows
    public List<Card> physicalCardsInHand = new List<Card>();

    [SerializeField] private CombatProperties combatProperties = null;
    [SerializeField] private int MaxHandDraw = 5;// Draw up to 5 cards every start of turn
    [SerializeField] private float DrawDelay = .15f;// Apply a small delay between each draw
    private LineRenderer arrowRenderer; // the renderer that will be used to draw the arrow of when player is aiming at target
    #endregion

    //--------------------------------------------------

    #region References
    private CombatPlayer combatPlayer;
    private Deck Deck;// Reference the deck script to access its list of cards to draw every start of turn
    #endregion

    //-------------------------------------------------

    #region Booleans
    public bool isDrawing { get; set; } = false;
    public bool isDragging { get; set; } = false;
    private bool createdArrow = false; //if arrow is created right now
    public bool isAiming = false; //if player is aiming with a TargetCard
    #endregion

   

    #region Events
    public Action playerDrawFromDeck;   //Event that gets called when player draws a card from deck.
    #endregion



    #region Event Subscription and Unsubscription and Variable Initialization
    private void OnDisable()
    {
        combatPlayer.OnMouseEnterCard -= HighlightCard;
        combatPlayer.OnMouseExitCard -= UnhighlightCard;
        SaveLoad.LoadEvent -= LoadHand;
        TurnManager.PlayerTurnStart -= DrawHand;
    }
    public override void Awake()
    {
        base.Awake();
        SaveLoad.LoadEvent += LoadHand;// Subscribes this method to the event to load the hand state stored on the save file
        TurnManager.PlayerTurnStart += DrawHand;// Subscribes thid method to the beginning of the player turn to draw up to five cards when called
    }
    void Start()
    {
        combatPlayer = GetComponent<CombatPlayer>();
        Deck = GetComponent<Deck>();// Reference is defined
        //////////Initialization of event /////////////
        combatPlayer.OnMouseEnterCard += HighlightCard;
        combatPlayer.OnMouseExitCard += UnhighlightCard;
        combatPlayer.OnCardSelected += DisableHoverEffects;
        combatPlayer.OnCardUnselected += ReenableHoverEffects;
        combatPlayer.OnCardUnselected += UnhighlightCard;
        combatPlayer.OnCardUnselected += UndoAimAtTarget;
        combatPlayer.OnTargetCardUsed += ReenableHoverEffects;
        combatPlayer.OnTargetCardUsed += UndoAimAtTarget;
        combatPlayer.OnNonTargetCardUsed += ReenableHoverEffects;
       
        //////////////////////////////////////////////////
        isDrawing = false;
        isDragging = false;
        createdArrow = false;
        isAiming = false;
    }
    #endregion

    void Update()// Moves cards every frame depending on the situation
    {
        MoveCards();
        DragSelectedCard();
    }
    public void LoadHand()// Load the data stored on the save file into the hand card list
    {
        List<int> IDList = GameData.Current.CardsinHandID;// Pulls the information from the loaded save
        List<CardInfo> TemporaryList = cardDatabase.GameCards;// Copies the card database list of card
        CardInfo CardToReceive=null;// Initializes the card to receive to be an empty class
        foreach(int ID in IDList)// Go through each stored card on the save
        {
            if (ID>=0)// If it is not a null card
            {
                CardToReceive = TemporaryList[ID];// Receives a card info based on its ID
                CardInfo cardInfoInstance = UnityEngine.Object.Instantiate(CardToReceive);// Creates a copy of that card info
               // cardsList.Add(cardInfoInstance);
                ReceiveCard(cardInfoInstance, this);// Add this card to the hand
            }
        }
    }
    #region Draw Methods

    public void DrawHand()// Draw a set of cards at the start of every plyer turn to get back to a maximum given value
    {
        var Amount = MaxHandDraw - physicalCardsInHand.Count;// Draw cards based on how many cards you need 
        Debug.Log(Amount);
        DrawCards(Amount);
    }
    public void DrawCards(int Amount)=> StartCoroutine(DrawCardDelay(Amount));// Call the coroutine that will add the cards to the hand and apply some delay// Draw a given Amount of cards
    IEnumerator DrawCardDelay(int Amount)// Add card has a small delay between each card to ensure a visual effec
    {
        while (Amount > 0)// While there are cards to be drawn
        {
            Deck.SendCard(Deck.cardsList[0], this);// Draw the first card on the deck card list
            Amount--;
            yield return new WaitForSeconds(DrawDelay);// Apply delay
        }
        yield break;
    }
    #endregion

    #region Receive and Send Card from and to Hand
    public override void ReceiveCard(CardInfo cardToReceive, CardPile origin)
    {
        base.ReceiveCard(cardToReceive, origin);
        if(origin.name == "Combat Player")
        {
            playerDrawFromDeck?.Invoke();   // Raise the player drawn event
            isDrawing = true; // turn isDrawing true
            //Spawn card
            Card cardSpawned = SpawnCardFromDeck(cardToReceive);
            //Add target for card to follow
            AddCardTarget(cardSpawned);
            //Update the targets positions based on how many cards/targets there are in hand right now
            UpdateTargets();
        }
    }

    public override void SendCard(CardInfo cardToSend, CardPile target)
    {
        Debug.Log(cardsList[0]);
        base.SendCard(cardToSend, target);
        OnCardRemoved(cardToSend);
    }

    private void OnCardRemoved(CardInfo cardBeingRemoved)
    {
        if(cardBeingRemoved != null)
        {
            physicalCardsInHand.Remove(cardBeingRemoved.MyPhysicalCard); //When the card is removed from hand, remove it from the list of physical cards in hand
            RemoveCardTarget(cardBeingRemoved.MyPhysicalCard); //Remove the card's PositionToFollow
            //TODO: Should probably happen an animation before destroying the card, like it going to the CD Pile, as in slay the spire
            GameObject.Destroy(cardBeingRemoved.MyPhysicalCard.gameObject); //Destroy the card gameObject
            if(physicalCardsInHand.Count > 0)
                UpdateTargets(); //Update the targets in hand if there is any
            cardBeingRemoved.MyPhysicalCard = null; // turn the myPhysicalCard field from the cardInfo null
        }
    }

    private Card SpawnCardFromDeck(CardInfo cardToSpawn)
    {
        GameObject cardSpawned =  GameObject.Instantiate(cardToSpawn.cardPrefab, cardDrawPosition.position, Quaternion.identity, handAnchor); //Spawn card when drawn from deck
        cardSpawned.GetComponent<Card>().cardInfo = cardToSpawn;  //Link the Card with it's respective CardInfo
        cardSpawned.GetComponent<Card>().followCardPositionToFollow = true;   //Allow card to follow the target and go to it's right position in hand
        cardToSpawn.MyPhysicalCard = cardSpawned.GetComponent<Card>();
        physicalCardsInHand.Add(cardSpawned.GetComponent<Card>()); //Adds the Card to the list of physical cards in hand
        return cardSpawned.GetComponent<Card>();
    }
    #endregion

    #region Card movement in Hand
    private void MoveCards()    // Moves the cards torward their targets smoothly
    {
        foreach (Card card in physicalCardsInHand)
        {
            if (card.followCardPositionToFollow) //The movement of the cards: They follow the cardPositionToFollow with interpolation method, so the movement is smooth. These "targets" are changed in other places
            {
                card.transform.position = Vector3.Lerp(card.transform.position, cardPositionsToFollow[card].position, Time.deltaTime * combatProperties.cardDrawingSpeed);
                card.transform.rotation = Quaternion.Slerp(card.transform.rotation, cardPositionsToFollow[card].rotation, Time.deltaTime * combatProperties.cardRotationSpeed);
            }
        }
    }

    //The default position of any created PositionToFollow is the center of the hand.
    //As the PositionsToFollow are updated in hand every time a card is drawn in the drawn method, the position will automatically be adjusted to the right position
    private void AddCardTarget(Card card)     //Adds a target to the card being drawn
    {
        CardPositionToFollow target = new CardPositionToFollow(HandAnchor.transform.position, Quaternion.identity); 
        cardPositionsToFollow.Add(card, target);
    }

    private void RemoveCardTarget(Card targetCard)
    {
        cardPositionsToFollow.Remove(targetCard);
    }

    private void UpdateTargets()
    {
        
        if(physicalCardsInHand.Count == 0)
        {
            throw new Exception("trying to update targets but there's no physical cards in hand, some problem with SpawnCard method");
        }
        else if(physicalCardsInHand.Count > 0)
        {
            //Here we need to move and rotate the card targets
            MoveAndRotateTargets();
        }
    }

    //----------------------------------------------------------------------------------------------------------
    //Explanation: 
    //For even number of PositionsToFollow:
    //gets the firstRightIndex, which from [0,1,2,3,4,5] would be 3. then the 3 and 2 would have to move half the distance betweencards but in opposite directions,
    //and the other cards need to move 1,5 + (multiples of 1, increasing with the distance from the center index of it's side).
    //----------------------------------------------
    //For odd number of PositionsToFollow:
    //The central card stays in the center of the hand. The other cards move to the sides with multiples of the distance between cards
    private void MoveAndRotateTargets()
    {
        int numberOfTargets = cardPositionsToFollow.Count;
        if (cardPositionsToFollow.Count % 2 == 0) //numero par de targets
        {
            int firstRightIndex = physicalCardsInHand.Count / 2;
            for (int i = 0; i < physicalCardsInHand.Count; i++)
            {
                if(i == firstRightIndex)
                {
                    cardPositionsToFollow[physicalCardsInHand[i]].position = HandAnchor.position + new Vector3(0.5f * combatProperties.offsetBetweenCards, 0f, -0.1f);
                    cardPositionsToFollow[physicalCardsInHand[i]].rotation = Quaternion.identity;
                }
                else if(i == firstRightIndex - 1)
                {
                    cardPositionsToFollow[physicalCardsInHand[i]].position = HandAnchor.position + new Vector3(-0.5f * combatProperties.offsetBetweenCards, 0f, +0.1f);
                    cardPositionsToFollow[physicalCardsInHand[i]].rotation = Quaternion.identity;
                }
                else if(i < firstRightIndex - 1)
                {
                    cardPositionsToFollow[physicalCardsInHand[i]].position = HandAnchor.position + new Vector3(-1.5f * combatProperties.offsetBetweenCards + (i - firstRightIndex + 2) * combatProperties.offsetBetweenCards, (i - firstRightIndex + 1) * combatProperties.cardsHeightDiff, Mathf.Abs(i - firstRightIndex) * 0.1f);
                    cardPositionsToFollow[physicalCardsInHand[i]].rotation = Quaternion.Euler(0f, 0f, 0f - (i-firstRightIndex + 1) * combatProperties.angleBetweenCards);
                }
                else if(i > firstRightIndex)
                {
                    cardPositionsToFollow[physicalCardsInHand[i]].position = HandAnchor.position + new Vector3(1.5f * combatProperties.offsetBetweenCards + (i - firstRightIndex - 1) * combatProperties.offsetBetweenCards, -(i - firstRightIndex) * combatProperties.cardsHeightDiff, -(i - firstRightIndex + 1) * 0.1f);
                    cardPositionsToFollow[physicalCardsInHand[i]].rotation = Quaternion.Euler(0f, 0f, 0f - (i-firstRightIndex) * combatProperties.angleBetweenCards);
                }
            }
        }
        else //numero ímpar de targets
        {
            int centralCardIndex = (int) Mathf.Floor(physicalCardsInHand.Count/2);
            //Sets the central card position to center of hand and rotation to identity and other cards change according to their index
            for(int i=0; i < physicalCardsInHand.Count; i++)
            {
                cardPositionsToFollow[physicalCardsInHand[i]].position = HandAnchor.position + new Vector3((i - centralCardIndex) * combatProperties.offsetBetweenCards, -Mathf.Abs(i - centralCardIndex) * combatProperties.cardsHeightDiff, (i - centralCardIndex) * (-0.1f));
                cardPositionsToFollow[physicalCardsInHand[i]].rotation = Quaternion.Euler(0f,0f,0f - (i - centralCardIndex) * combatProperties.angleBetweenCards);
            }
        }
    }

    //----------------------------------------------------------------------------------------
    #endregion

    #region MouseHover
    //Function that highlights the card when hovered over. It is called in the event onMouseEnter that was created in the combatPlayer script.
    //It takes the card from the call and exposes it in front of others and upscale it, as well as move the adjacent cards apart from the highlighted, for a neat effect. Subscription in the initialization region
    private void HighlightCard(GameObject card)
    {
        if (!card.GetComponent<Card>().highlighted)
        {
            card.GetComponent<Card>().highlighted = true;
            for (int i = 0; i < physicalCardsInHand.Count; i++)
            {
                //Highlight the card
                if (physicalCardsInHand[i] == card.GetComponent<Card>())
                {
                    cardPositionsToFollow[physicalCardsInHand[i]].position += new Vector3(0f, 0f, -1.5f);
                    if ((card.transform.position - cardPositionsToFollow[physicalCardsInHand[i]].position).magnitude < 2f)
                    {
                        card.transform.position = cardPositionsToFollow[physicalCardsInHand[i]].position;
                    }
                    card.transform.localScale = new Vector3(1f, 1f, 1f) * combatProperties.cardHighlightScale;
                }

                //Move adjacent cards to improve highlight of the hovered card
                if (i + 1 < physicalCardsInHand.Count)
                {
                    if (physicalCardsInHand[i + 1] == card.GetComponent<Card>()) // i é o index da carta da esquerda nesse caso
                    {
                        cardPositionsToFollow[physicalCardsInHand[i]].position += new Vector3(-0.3f, 0f, 0f);
                    }
                }
                if (i - 1 >= 0)
                {
                    if (physicalCardsInHand[i - 1] == card.GetComponent<Card>())
                    {
                        cardPositionsToFollow[physicalCardsInHand[i]].position += new Vector3(0.3f, 0f, 0f);
                    }
                }
            }
        }
    }

    //Function that undo what highlightCard did. This is called in the event OnMouseExit that was created in the combatPlayer script. Subscription in the initialization region
    private void UnhighlightCard (GameObject card)
    {
        if (card.GetComponent<Card>().highlighted)
        {
            card.GetComponent<Card>().highlighted = false;
            for (int i = 0; i < physicalCardsInHand.Count; i++)
            {
                //Unhighlight the card
                if (physicalCardsInHand[i] == card.GetComponent<Card>())
                {
                    cardPositionsToFollow[physicalCardsInHand[i]].position += new Vector3(0f, 0f, 1.5f);
                    if ((card.transform.position - cardPositionsToFollow[physicalCardsInHand[i]].position).magnitude < 2f)
                    {
                        card.transform.position = cardPositionsToFollow[physicalCardsInHand[i]].position;
                    }
                    card.transform.localScale = new Vector3(1f, 1f, 1f) * combatProperties.cardNormalScale;
                }

                //undo the move of adjacent cards
                if (i + 1 < physicalCardsInHand.Count)
                {
                    if (physicalCardsInHand[i + 1] == card.GetComponent<Card>()) // i é o index da carta da esquerda nesse caso
                    {
                        cardPositionsToFollow[physicalCardsInHand[i]].position += new Vector3(+0.3f, 0f, 0f);
                    }
                }
                if (i - 1 >= 0)
                {
                    if (physicalCardsInHand[i - 1] == card.GetComponent<Card>())    // i é o index da carta da direita nesse caso
                    {
                        cardPositionsToFollow[physicalCardsInHand[i]].position += new Vector3(-0.3f, 0f, 0f);
                    }
                }
            }
        }
    }

    //TODO: Will move the selected card, but the atatck card will have a different behaviour than the cards such as block, heal etc.
    //(attack will draw an arrow to the enemy and the others will have their effects once the player drops them outside the hand zone)
    private void DragSelectedCard() 
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        foreach (Card card in physicalCardsInHand)
        {
            if(card.selected)
            {
                if (card.type == "TargetCard")
                {
                    if (combatPlayer.IsMouseInHandZone() && !createdArrow)  //if card is of type TargetCard and is not aiming at target yet, just drag card
                    {
                        card.followCardPositionToFollow = false;
                        card.transform.position = new Vector3(mousePos2D.x, mousePos2D.y, combatProperties.zAxisOffsetWhenCardDrag); // Makes the card follow the mouse position, but with offset of 1 in the Z axis so the card is exposed in front of other objects in the scene
                        card.transform.rotation = Quaternion.identity;
                        
                    }
                    else //if( card is not in hand zone anymore, it means you want to aim at target
                    {
                        card.transform.position = Vector3.Lerp(card.transform.position, HandAnchor.position + new Vector3(0f,0f,-1f), Time.deltaTime * combatProperties.cardDrawingSpeed); // Makes card go to the hand's center position smoothly and with an offset in the z axis
                        card.transform.rotation = Quaternion.identity;
                        //Draw arrow
                        AimAtTarget(card, mousePos2D);
                        
                    }
                }
                else if(card.type == "NonTargetCard")
                {
                    if(combatPlayer.IsMouseInHandZone()) //When Non Target Card is inside hand zone , just drag
                    {
                        card.followCardPositionToFollow = false;
                        card.transform.position = new Vector3(mousePos2D.x, mousePos2D.y, combatProperties.zAxisOffsetWhenCardDrag); // Makes the card follow the mouse position, but with offset of 1 in the Z axis so the card is exposed in front of other objects in the scene
                        card.transform.rotation = Quaternion.identity;
                    }
                    else //When Non Target Card is inside hand zone , just drag as well
                    {
                        card.followCardPositionToFollow = false;
                        card.transform.position = new Vector3(mousePos2D.x, mousePos2D.y, combatProperties.zAxisOffsetWhenCardDrag); // Makes the card follow the mouse position, but with offset of 1 in the Z axis so the card is exposed in front of other objects in the scene
                        card.transform.rotation = Quaternion.identity;
                    }
                }
                else
                {
                    throw new Exception("this card doesn't have a valid type");
                }
            }
        }
    }

    private void AimAtTarget(Card card, Vector2 mousePos2D)   //draws the arrow for when player is aiming at a target with TargetCard
    {
        isAiming = true; //To tell the game that the player is aiming a Target card at someone
        //Draw Arrow
        if(!createdArrow)
        {
            GameObject line = new GameObject();
            line.transform.position = card.transform.position;
            line.AddComponent<LineRenderer>();
            arrowRenderer = line.GetComponent<LineRenderer>();
            createdArrow = true;
        }
        arrowRenderer.SetPosition(0, card.transform.position); // sets the origin position of the line renderer to be the card transform position. TODO: put this origin position in the top of the card
        arrowRenderer.SetPosition(1, new Vector3(mousePos2D.x, mousePos2D.y, 1f)); // sets the end position of the line renderer to be in the mouse pointer position.
    }

    private void UndoAimAtTarget(GameObject card) //Destroy the arrow
    {
        if (card.GetComponent<Card>().type == "TargetCard")
        {
            if (arrowRenderer != null)
            {
                GameObject.Destroy(arrowRenderer.gameObject);
                createdArrow = false;
            }
        }
    }

    private void DisableHoverEffects(GameObject card)
    {
        combatPlayer.OnMouseEnterCard -= HighlightCard;
        combatPlayer.OnMouseExitCard -= UnhighlightCard;
    }

    private void ReenableHoverEffects(GameObject card)
    {
        StartCoroutine("Wait");           //COROUTINE NOT WORKING
        Debug.Log("chamou corrotina?");
        combatPlayer.OnMouseEnterCard += HighlightCard;
        combatPlayer.OnMouseExitCard += UnhighlightCard;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(4f);
    }
    #endregion
}
