using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CDPile : CardPile
{
    [SerializeField] private List<GameObject> cardsCD_Completed = new List<GameObject>(); // list of cards with cooldown completed
    [SerializeField] private Transform cardOffCDPosition = null; //Point where cards are created when drawn
    public List<GameObject> CD0 = new List<GameObject>();
    public List<GameObject> CD1 = new List<GameObject>();
    public List<GameObject> CD2 = new List<GameObject>();
    public List<GameObject> CD3plus = new List<GameObject>();
    public Transform CardOffCDPosition  //Getter
    {
        get
        {
            return cardOffCDPosition;
        }
    }

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

    public override void ReceiveCard(GameObject cardToReceive, CardPile origin)
    {
        base.ReceiveCard(cardToReceive, origin);
        cardToReceive.GetComponent<VirtualCard>().CurrentCooldownTime = cardToReceive.GetComponent<VirtualCard>().cardInfo.Cooldown;
        cardToReceive.transform.parent = cardOffCDPosition;
        cardToReceive.GetComponent<VirtualCard>()?.TurnVirtual();       
        switch(cardToReceive.GetComponent<VirtualCard>()?.cardInfo.Cooldown)
        {
            case 0:
                CD0.Add(cardToReceive);
                break;
            case 1:
                CD1.Add(cardToReceive);
                break;
            case 2:
                CD2.Add(cardToReceive);
                break;
            default:
                if(cardToReceive.GetComponent<VirtualCard>().cardInfo.Cooldown >=3)
                    CD3plus.Add(cardToReceive);
                break;
        }
        if(origin.PileName=="Deck")// If this card came from the deck
            if (cardToReceive.GetComponent<VirtualCard>().virtualCardExtensions.ContainsKey("Wildcast"))// If this card has a Wild Cast effect
            {
                Debug.Log("Wildcast");
                cardToReceive.GetComponent<VirtualCard>().virtualCardExtensions["Wildcast"].ExtensionEffect();// Execute its Wild Cast effect
            }
    }

    public override void SendCard(GameObject cardToSend, CardPile target)
    {
        base.SendCard(cardToSend, target);
        switch (cardToSend.GetComponent<VirtualCard>().CurrentCooldownTime)
        {
            case 0:
                CD0.Remove(cardToSend);
                break;
            case 1:
                CD1.Remove(cardToSend);
                break;
            case 2:
                CD2.Remove(cardToSend);
                break;
            default:
                if (cardToSend.GetComponent<VirtualCard>().CurrentCooldownTime >= 3)
                {
                    CD3plus.Remove(cardToSend);
                }
                break;
        }
    }

    public void UpdateCooldown()
    {
        anyCardCompletedCD = false;
        if(!Player.Disrupted)// If not disrupted, card's CD's are updated 
            for(int i = cardsList.Count-1; i >= 0; i--)
            {
                if (cardsList[i].GetComponent<VirtualCard>()?.CurrentCooldownTime > 0) // if card still on cooldown
                {
                    #region Update CD lists
                    switch (cardsList[i].GetComponent<VirtualCard>().CurrentCooldownTime)
                    {
                        case 1:
                            CD1.Remove(cardsList[i]);
                            CD0.Add(cardsList[i]);
                            break;
                        case 2:
                            CD2.Remove(cardsList[i]);
                            CD1.Add(cardsList[i]);
                            break;
                        default:
                            if (cardsList[i].GetComponent<VirtualCard>().CurrentCooldownTime == 3)
                            {
                                CD3plus.Remove(cardsList[i]);
                                CD2.Add(cardsList[i]);
                            }
                            break;
                    }
                    #endregion
                    cardsList[i].GetComponent<VirtualCard>().CurrentCooldownTime -= 1; //update the cooldown reducing 1 in the currentCooldownTime
                }
                else //if any card completed it's cooldown
                {
                    cardsCD_Completed.Add(cardsList[i]);// add card to list with all the cards that have completed the cooldown
                    CD0.Remove(cardsList[i]); // Update CD list
                    cardsList.RemoveAt(i);
                    //Raise shuffle flag
                    anyCardCompletedCD = true;
                }
            }
    }
    public void UpdateCooldown(VirtualCard TargetCard)
    {
        if (!Player.Disrupted)// If not disrupted, card's CD's are updated 
            if(TargetCard.CurrentCooldownTime>0)// If this card has 0 CD, it won't update its CD
            {
                #region Update CD lists
                switch (TargetCard.CurrentCooldownTime)// Process the card's CD to determine which list it will be 
                {
                    case 1:// CD reducing from 1 to 0
                        CD1.Remove(TargetCard.gameObject);// Remove from the CD=1 List
                        CD0.Add(TargetCard.gameObject);// Add it to the CD=0 List
                        break;
                    case 2:// CD Reducing from 2 to 1
                        CD2.Remove(TargetCard.gameObject);// Remove from CD=2 List
                        CD1.Add(TargetCard.gameObject);// Add it to the CD=1 List
                        break;
                    default:// CD Reducing from X to X-1, when X>=3
                        if (TargetCard.CurrentCooldownTime == 3)// Only move to the CD=2 List if this card's CD is currently 3
                        {
                            CD3plus.Remove(TargetCard.gameObject);// Remove from the X List
                            CD2.Add(TargetCard.gameObject);// Add it to the CD=2 List
                        }
                        break;
                }
                #endregion
                TargetCard.CurrentCooldownTime -= 1; //update the cooldown reducing 1 in the currentCooldownTime
            }
            else
            {
                cardsCD_Completed.Add(TargetCard.gameObject);// add card to list with all the cards that have completed the cooldown
                CD0.Remove(TargetCard.gameObject); // Update CD list
                cardsList.Remove(TargetCard.gameObject);// Remove this
                //Raise shuffle flag
                anyCardCompletedCD = true;// If this flag is true, the deck will be shuffled at the end of the player's start turn event
            }
    }
    public void SendCardsBackToDeckAndShuffle()
    {
        for (int i = cardsCD_Completed.Count - 1; i >= 0; i--)
        {
            SendCard(cardsCD_Completed[i], playerDeck);
            Debug.Log(cardsCD_Completed[i].name);
            if (cardsCD_Completed[i].GetComponent<VirtualCard>().virtualCardExtensions.ContainsKey("Overflow"))// If this card has an Overflow effect
            {
                Debug.Log("Overflowing");
                cardsCD_Completed[i].GetComponent<VirtualCard>().virtualCardExtensions["Overflow"].ExtensionEffect();// Execute its overflow effect
            }
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
                GameObject cardInstance = GameObject.Instantiate(CardToReceive.cardPrefab, CardOffCDPosition); // Creates an instance of that card prefab
                cardInstance.GetComponent<VirtualCard>()?.TurnVirtual();
                cardInstance.GetComponent<VirtualCard>().CurrentCooldownTime = CombatGameData.Current.CardsCD[iterator];// Pairs the card info to its CD
                cardsList.Add(cardInstance);// Add it to the list of card infos
                iterator++;// Increment the iterator
            }
        }
    }
}
