using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDPile : CardPile
{
    [SerializeField] private List<CardInfo> cardsCD_Completed = new List<CardInfo>(); // list of cards with cooldown completed
    #region References
    private Deck playerDeck;
    #endregion

    #region Booleans
    [SerializeField] private bool anyCardCompletedCD; // this is also the shuffle flag
    #endregion

    private void Awake()
    {
        //Event subscribing
    
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
            Debug.Log("mandou carta");
            SendCard(cardsCD_Completed[i], playerDeck);
            cardsCD_Completed.RemoveAt(i);
        }
        //TODO: Coroutine with cards going to deck animation
        if(anyCardCompletedCD)
        {
            Shuffle();
        }
    }
}
