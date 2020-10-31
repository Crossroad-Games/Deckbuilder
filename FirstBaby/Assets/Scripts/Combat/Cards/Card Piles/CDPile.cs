using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDPile : CardPile
{
    [SerializeField] private List<CardInfo> cardsCD_Completed = new List<CardInfo>(); // list of cards with cooldown completed
    #region References
    private Deck playerDeck;
    private CombatPlayer Player;
    #endregion

    #region Booleans
    [SerializeField] private bool anyCardCompletedCD; // this is also the shuffle flag
    #endregion

    public override void Awake()
    {
        base.Awake();
        SaveLoad.LoadEvent += LoadSave;// Subscribe to the load event from the saveload script
        Player = transform.parent.gameObject.GetComponent<CombatPlayer>();// Reference to the player
    }
    private void OnDisable()
    {
        SaveLoad.LoadEvent -= LoadSave;// Subscribe to the load event from the saveload script
    }
    // Start is called before the first frame update
    void Start()
    {
        playerDeck = transform.parent.GetComponent<Deck>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ReceiveCard(CardInfo cardToReceive, CardPile origin)
    {
        base.ReceiveCard(cardToReceive, origin);
        cardToReceive.CurrentCooldownTime = cardToReceive.Cooldown;
    }

    public void UpdateCooldown()
    {
        anyCardCompletedCD = false;
        Debug.Log(Player.Disrupted);
        if(!Player.Disrupted)// If not disrupted, card's CD's are updated 
            for(int i = cardsList.Count-1; i >= 0; i--)
            {
                if (cardsList[i].CurrentCooldownTime > 0) // if card still on cooldown
                {
                    cardsList[i].CurrentCooldownTime -= 1; //update the cooldown reducing 1 in the currentCooldownTime
                }
                else //if any card completed it's cooldown
                {
                    cardsCD_Completed.Add(cardsList[i]);// add card to list with all the cards that have completed the cooldown
                    cardsList.RemoveAt(i);
                    //Raise shuffle flag
                    anyCardCompletedCD = true;
                }
            }
    }

    public void SendCardsBackToDeckAndShuffle()
    {
        for (int i = cardsCD_Completed.Count - 1; i >= 0; i--)
        {
            SendCard(cardsCD_Completed[i], playerDeck);
            cardsCD_Completed.RemoveAt(i);
        }
        //TODO: Coroutine with cards going to deck animation
        if(anyCardCompletedCD)
        {
            Shuffle();
        }
    }
    public void LoadSave()// Process the saved information in the save file
    {
        List<int> IDList = CombatGameData.Current.CardsinCD;// Pulls the information from the loaded save
        List<CardInfo> TemporaryList = cardDatabase.GameCards;// Copies the card database list of card
        CardInfo CardToReceive = null;// Initializes the card to receive to be an empty class
        var iterator = 0;
        foreach (int ID in IDList)// Go through each stored card on the save
        {
            if (ID >= 0)// If it is not a null card
            {
                CardToReceive = TemporaryList[ID];// Cardinfo is chosen based on its ID
                CardInfo cardInfoInstance = UnityEngine.Object.Instantiate(CardToReceive);// Creates an instance of that card info
                cardInfoInstance.CurrentCooldownTime = CombatGameData.Current.CardsCD[iterator];// Pairs the card info to its CD
                cardsList.Add(cardInfoInstance);// Add it to the list of card infos
                iterator++;// Increment the iterator
            }
        }
    }
}
