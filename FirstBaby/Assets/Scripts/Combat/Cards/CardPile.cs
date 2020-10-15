using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPile : MonoBehaviour
{
    public List<CardInfo> cardsList = new List<CardInfo>();    //List if cards in this CardPile

    #region References
    protected CardDatabase cardDatabase;
    protected CombatManager combatManager;
    #endregion

    public virtual void Awake()
    {
        cardDatabase = Object.FindObjectOfType<CardDatabase>(); // Gets the reference of the card database with all the available cards in the game
        combatManager = Object.FindObjectOfType<CombatManager>(); // Gets the reference of the combat manager.
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public virtual void SendCard(CardInfo cardToSend, CardPile target)  //Method that sends card from this card pile to another
    {
        cardsList.Remove(cardToSend);  //Remove from this Pile's card list.
        target.ReceiveCard(cardToSend, this); //Makes the target cardPile receive the card
    }

    public virtual void ReceiveCard(CardInfo cardToReceive, CardPile origin) // Method that receives a card.
    {
        cardsList.Add(cardToReceive);   //Add to this Pile's card list.
    }

    public void Shuffle()
    {
        int numberOfCards = cardsList.Count;       //Take the number of cards in deck and put build a list with this number of elements as ints in ascending order
        List<int> cardsIndex = new List<int>();         // Auxiliar list for randomizing new positions for the cards
        List<CardInfo> cardsListCopy = new List<CardInfo>(cardsList);      //Creates a copy of the Deck info
        for (int i = 0; i < numberOfCards; i++)
        {
            cardsIndex.Add(i);
        }

        // imagine the positions [1,2,3,4,5,6,7,8,9] (9 cards in deck) , you take a random one, 5 for example, that's the position where the card at index i = 0 from pile will go,
        // then remove that position = [1,2,3,4,6,7,8,9] , randomize again and pick 9 for example, that's the position where the card at index i = 1 from pile will go.
        for (int i = 0; i < cardsList.Count; i++) //shuffle the copy of card pile
        {
            int r = Random.Range(0, cardsIndex.Count - 1); 
            int random = cardsIndex[r]; //Gets a random int from the ones in the cardsIndex list, this index is where the card of the current iteration of for will go in the shuffled deck
            cardsIndex.RemoveAt(r); //get rid of the already used Index so there won't be two cards going to the same position. (everytime you randomize an index, the previous index is not available anymore
            cardsListCopy.RemoveAt(random);
            cardsListCopy.Insert(random, cardsList[i]);        //Take the card from deck at index i and put it into the deck copy at the new index.  Can't simply update deck directly because you would lose info of the replaced card.
        }

        for (int i = 0; i < cardsList.Count; i++) //Fill the cardPile itself with the shuffled copy
        {
            cardsList.RemoveAt(i);
            cardsList.Insert(i, cardsListCopy[i]);
        }
    }
}
