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
        TurnManager.CombatStart += LoadDeck;// Subscribe to this event to load the save file deck information into this list
        TurnManager.CombatStart += Shuffle;// Subscribe to this event to shuffle the deck at the start of combat
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnDisable()
    {
        TurnManager.CombatStart += LoadDeck;// Unsubscribe to this event to load the save file deck information into this list
        TurnManager.CombatStart -= Shuffle;// Unsubscribe to this event to shuffle the deck at the start of combat
    }
    public void LoadDeck()// Loads the deck from data on the save file
    {
        List<int> IDList = CombatGameData.Current.CardsinDeckID;// Pulls the information from the loaded save
        foreach (int ID in IDList)
            Debug.Log(ID);
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
