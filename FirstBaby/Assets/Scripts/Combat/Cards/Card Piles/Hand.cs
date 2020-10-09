using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : CardPile
{
    [SerializeField] private Transform handAnchor; //Pivot for card positions in hand
    public Transform HandAnchor //Getter
    {
        get
        {
            return handAnchor;
        }
    }
    [SerializeField] private Transform cardDrawPosition; //Point where cards are created when drawn
    public Transform CardDrawPosition  //Getter
    {
        get
        {
            return cardDrawPosition;
        }
    }

    public Action playerDrawFromDeck;   //Event that gets called when player draws a card from deck.

    public List<Card> physicalCardsInHand = new List<Card>();


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void ReceiveCard(CardInfo cardToReceive, CardPile origin)
    {
        base.ReceiveCard(cardToReceive, origin);
        if(origin.name == "Combat Player")
        {
            playerDrawFromDeck?.Invoke();
            SpawnCardWhenDraw(cardToReceive);
        }
    }

    private void SpawnCardWhenDraw(CardInfo cardToSpawn)
    {
        GameObject cardSpawned =  GameObject.Instantiate(cardToSpawn.cardPrefab, cardDrawPosition.position, Quaternion.identity, handAnchor); //Spawn card when drawn from deck
        cardSpawned.GetComponent<Card>().cardInfo = cardToSpawn;  //Link the Card with it's respective CardInfo
        physicalCardsInHand.Add(cardSpawned.GetComponent<Card>()); //Adds the Card to the list of physical cards in hand
    }
}
