using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : CardPile
{
    #region References
    [SerializeField] private CardPile hand;
    #endregion

    public override void Awake()
    {
        base.Awake();
        SaveLoad.LoadEvent += LoadDeck;// Subscribe to this event to load the save file deck information into this list
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnDisable()
    {
        SaveLoad.LoadEvent -= LoadDeck;// Unsubscribe
    }
    public void LoadDeck()// Loads the deck from data on the save file
    {
        List<int> IDList = GameData.Current.CardsinDeckID;// Pulls the information from the loaded save
        List<CardInfo> TemporaryList = cardDatabase.GameCards;// Copies the card database list of card
        CardInfo CardToReceive = null;// Initializes the card to receive to be an empty class
        foreach (int ID in IDList)// Go through each stored card on the save
        {
            if (ID >= 0)// If it is not a null card
            {
                CardToReceive = TemporaryList[ID];// Cardinfo is chosen based on its ID
                CardInfo cardInfoInstance = UnityEngine.Object.Instantiate(CardToReceive);// Creates an instance of that card info
                cardsList.Add(cardInfoInstance);// Add it to the list of card infos
            }
        }
    }
}
