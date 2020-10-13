using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hand : CardPile
{
    #region Properties
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
    public Dictionary<Card, CardTarget> cardTargets = new Dictionary<Card, CardTarget>();    //Entity that cards follows
    public List<Card> physicalCardsInHand = new List<Card>();

    [SerializeField] private CombatProperties combatProperties = null;
    #endregion

    //--------------------------------------------------

    #region References
    private CombatPlayer combatPlayer;
    #endregion

    //-------------------------------------------------

    #region Booleans
    private bool isDrawing { get; set; } = false;
    #endregion

    #region Events
    public Action playerDrawFromDeck;   //Event that gets called when player draws a card from deck.
    #endregion


    

    private void OnDisable()
    {
        combatPlayer.OnMouseEnterCard -= HighlightCard;
        combatPlayer.OnMouseExitCard -= UnhighlightCard;
    }

    void Start()
    {
        //Initialization
        combatPlayer = GetComponent<CombatPlayer>();
        combatPlayer.OnMouseEnterCard += HighlightCard;
        combatPlayer.OnMouseExitCard += UnhighlightCard;
        isDrawing = false;
    }

    void Update()
    {
        MoveCards();
        DragSelectedCard();
    }

    public override void ReceiveCard(CardInfo cardToReceive, CardPile origin)
    {
        base.ReceiveCard(cardToReceive, origin);
        if(origin.name == "Combat Player")
        {
            playerDrawFromDeck?.Invoke();   // Raise the player drawn event
            isDrawing = true;
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
        base.SendCard(cardToSend, target);
        OnCardRemoved(cardToSend);
    }

    private void OnCardRemoved(CardInfo cardBeingRemoved)
    {
        if(cardBeingRemoved != null)
        {
            physicalCardsInHand.Remove(cardBeingRemoved.MyPhysicalCard);
            RemoveCardTarget(cardBeingRemoved.MyPhysicalCard);
            GameObject.Destroy(cardBeingRemoved.MyPhysicalCard.gameObject);
            if(physicalCardsInHand.Count > 0)
                UpdateTargets();
            cardBeingRemoved.MyPhysicalCard = null;
        }
    }

    private Card SpawnCardFromDeck(CardInfo cardToSpawn)
    {
        GameObject cardSpawned =  GameObject.Instantiate(cardToSpawn.cardPrefab, cardDrawPosition.position, Quaternion.identity, handAnchor); //Spawn card when drawn from deck
        cardSpawned.GetComponent<Card>().cardInfo = cardToSpawn;  //Link the Card with it's respective CardInfo
        cardSpawned.GetComponent<Card>().followTarget = true;   //Allow card to follow the target and go to it's right position in hand
        cardToSpawn.MyPhysicalCard = cardSpawned.GetComponent<Card>();
        physicalCardsInHand.Add(cardSpawned.GetComponent<Card>()); //Adds the Card to the list of physical cards in hand
        return cardSpawned.GetComponent<Card>();
    }

    private void MoveCards()    // Moves the cards torward their targets smoothly
    {
        foreach (Card card in physicalCardsInHand)
        {
            if (card.followTarget && isDrawing)
            {
                card.transform.position = Vector3.Lerp(card.transform.position, cardTargets[card].position, Time.deltaTime * combatProperties.cardDrawingSpeed);
                card.transform.rotation = Quaternion.Slerp(card.transform.rotation, cardTargets[card].rotation, Time.deltaTime * combatProperties.cardRotationSpeed);
            }
        }
    }

    private void AddCardTarget(Card card)     //Adds a target to the card being drawn
    {
        CardTarget target = new CardTarget(HandAnchor.transform.position, Quaternion.identity);
        cardTargets.Add(card, target);
    }

    private void RemoveCardTarget(Card targetCard)
    {
        cardTargets.Remove(targetCard);
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

    private void MoveAndRotateTargets()
    {
        int numberOfTargets = cardTargets.Count;
        if (cardTargets.Count % 2 == 0) //numero par de targets
        {
            int firstRightIndex = physicalCardsInHand.Count / 2;
            for (int i = 0; i < physicalCardsInHand.Count; i++)
            {
                if(i == firstRightIndex)
                {
                    cardTargets[physicalCardsInHand[i]].position = HandAnchor.position + new Vector3(0.5f * combatProperties.offsetBetweenCards, 0f, -0.1f);
                    cardTargets[physicalCardsInHand[i]].rotation = Quaternion.identity;
                }
                else if(i == firstRightIndex - 1)
                {
                    cardTargets[physicalCardsInHand[i]].position = HandAnchor.position + new Vector3(-0.5f * combatProperties.offsetBetweenCards, 0f, +0.1f);
                    cardTargets[physicalCardsInHand[i]].rotation = Quaternion.identity;
                }
                else if(i < firstRightIndex - 1)
                {
                    cardTargets[physicalCardsInHand[i]].position = HandAnchor.position + new Vector3(-1.5f * combatProperties.offsetBetweenCards + (i - firstRightIndex + 2) * combatProperties.offsetBetweenCards, (i - firstRightIndex + 1) * combatProperties.cardsHeightDiff, Mathf.Abs(i - firstRightIndex) * 0.1f);
                    cardTargets[physicalCardsInHand[i]].rotation = Quaternion.Euler(0f, 0f, 0f - (i-firstRightIndex + 1) * combatProperties.angleBetweenCards);
                }
                else if(i > firstRightIndex)
                {
                    cardTargets[physicalCardsInHand[i]].position = HandAnchor.position + new Vector3(1.5f * combatProperties.offsetBetweenCards + (i - firstRightIndex - 1) * combatProperties.offsetBetweenCards, -(i - firstRightIndex) * combatProperties.cardsHeightDiff, -(i - firstRightIndex + 1) * 0.1f);
                    cardTargets[physicalCardsInHand[i]].rotation = Quaternion.Euler(0f, 0f, 0f - (i-firstRightIndex) * combatProperties.angleBetweenCards);
                }
            }
        }
        else //numero ímpar de targets
        {
            int centralCardIndex = (int) Mathf.Floor(physicalCardsInHand.Count/2);
            //Sets the central card position to center of hand and rotation to identity and other cards change according to their index
            for(int i=0; i < physicalCardsInHand.Count; i++)
            {
                cardTargets[physicalCardsInHand[i]].position = HandAnchor.position + new Vector3((i - centralCardIndex) * combatProperties.offsetBetweenCards, -Mathf.Abs(i - centralCardIndex) * combatProperties.cardsHeightDiff, (i - centralCardIndex) * (-0.1f));
                cardTargets[physicalCardsInHand[i]].rotation = Quaternion.Euler(0f,0f,0f - (i - centralCardIndex) * combatProperties.angleBetweenCards);
            }
        }
    }

    //----------------------------------------------------------------------------------------

    #region MouseHover
    private void HighlightCard(GameObject card)
    {
        for (int i=0; i < physicalCardsInHand.Count; i++)
        {
            //Highlight the card
            if(physicalCardsInHand[i] == card.GetComponent<Card>())
            {
                cardTargets[physicalCardsInHand[i]].position += new Vector3(0f, 0f, -1f);
                card.transform.position = cardTargets[physicalCardsInHand[i]].position;
                card.transform.localScale = new Vector3(1f,1f,1f) * combatProperties.cardHighlightScale;
            }

            //Move adjacent cards to improve highlight of the hovered card
            if (i + 1 < physicalCardsInHand.Count)
            {
                if (physicalCardsInHand[i + 1] == card.GetComponent<Card>()) // i é o index da carta da esquerda nesse caso
                {
                    cardTargets[physicalCardsInHand[i]].position += new Vector3(-0.3f, 0f, 0f);
                }
            }
            if (i - 1 >= 0)
            {
                if (physicalCardsInHand[i - 1] == card.GetComponent<Card>())
                {
                    cardTargets[physicalCardsInHand[i]].position += new Vector3(0.3f, 0f, 0f);
                }
            }
        }
    }

    private void UnhighlightCard (GameObject card)
    {
        for (int i = 0; i < physicalCardsInHand.Count; i++)
        {
            //Unhighlight the card
            if (physicalCardsInHand[i] == card.GetComponent<Card>())
            {
                cardTargets[physicalCardsInHand[i]].position += new Vector3(0f, 0f, 1f);
                card.transform.position = cardTargets[physicalCardsInHand[i]].position;
                card.transform.localScale = new Vector3(1f, 1f, 1f) * combatProperties.cardNormalScale;
            }

            //undo the move of adjacent cards
            if (i + 1 < physicalCardsInHand.Count)
            {
                if (physicalCardsInHand[i + 1] == card.GetComponent<Card>()) // i é o index da carta da esquerda nesse caso
                {
                    cardTargets[physicalCardsInHand[i]].position += new Vector3(+0.3f, 0f, 0f);
                    Debug.Log("jogou da esquerda mais pra esquerda");
                }
            }
            if (i - 1 >= 0)
            {
                if (physicalCardsInHand[i - 1] == card.GetComponent<Card>())    // i é o index da carta da direita nesse caso
                {
                    cardTargets[physicalCardsInHand[i]].position += new Vector3(-0.3f, 0f, 0f);
                    Debug.Log("jogou da direita mais pra direita");
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
            if(card.Selected)
            {
                card.followTarget = false;
                card.transform.position = new Vector3(mousePos2D.x, mousePos2D.y, 1f);
            }
        }
    }


    #endregion
}
