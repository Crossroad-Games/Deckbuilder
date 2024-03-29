﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : CardPile
{
    #region References
    [SerializeField] private Hand hand;
    #endregion

    public override void Awake()
    {
        base.Awake();
        PileName = "Deck";
        SaveLoad.LoadEvent += LoadDeck;// Subscribe to this event to load the save file deck information into this list
        SaveLoad.LoadEvent += Shuffle;// Subscribe to this event to shuffle the deck at the start of combat
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void ReceiveCard(GameObject cardToReceive, CardPile origin)
    {
        base.ReceiveCard(cardToReceive, origin);
        cardToReceive.transform.parent = hand.CardDrawPosition;
        cardToReceive.GetComponent<VirtualCard>()?.TurnVirtual();
    }

    private void OnDisable()
    {
        SaveLoad.LoadEvent -= LoadDeck;// Unsubscribe to this event to load the save file deck information into this list
        SaveLoad.LoadEvent -= Shuffle;// Unsubscribe to this event to shuffle the deck at the start of combat
    }
    public void LoadDeck()// Loads the deck from data on the save file
    {
        List<int> IDList = CombatGameData.Current.CardsinDeckID;// Pulls the information from the loaded save
        List<CardInfo> TemporaryList = cardDatabase.GameCards;// Copies the card database list of card
        CardInfo CardToReceive = null;// Initializes the card to receive to be an empty class
        foreach (int ID in IDList)// Go through each stored card on the save
        {
            if (ID >= 0)// If it is not a null card
            {
                CardToReceive = TemporaryList[ID];// Cardinfo is chosen based on its ID
                GameObject cardInstance = GameObject.Instantiate(CardToReceive.cardPrefab, hand.CardDrawPosition); // Creates an instance of that card prefab
                cardInstance.GetComponent<PhysicalCard>().CardLevel = DungeonGameData.Current.PlayerData.CardLevels[ID];// Sets the card level based on ID
                var VirtualCard = cardInstance.GetComponent<VirtualCard>();
                VirtualCard.CardLevel = DungeonGameData.Current.PlayerData.CardLevels[ID];// Sets the card level based on ID
                VirtualCard.PhysicalCardBehaviour.CardLevel = VirtualCard.CardLevel;// Sets the card level based on ID
                VirtualCard.PhysicalCardBehaviour.LevelRanks();// Apply the LVL updates
                cardsList.Add(cardInstance);// Add it to the list of card infos
                VirtualCard?.TurnVirtual();
            }
        }
        
    }
}
