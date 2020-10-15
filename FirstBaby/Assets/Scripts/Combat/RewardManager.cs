using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    private CardDatabase cardDatabase;
    [Header("UI elements")]
    [SerializeField] private GameObject CardSelectionUI = null;
    [SerializeField] private List<Button> cardOptionsButtons = null;

    #region References
    private Deck playerDeck;
    #endregion

    private Dictionary<Button, CardInfo> cardOptions = new Dictionary<Button, CardInfo>();


    private void Awake()
    {
        //Initialization
        cardDatabase = GameObject.Find("Card Database").GetComponent<CardDatabase>();
        //--------------------
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region Card Selectiion
    public void FillCardSelection()
    {
        ClearCardOptions();
        List<CardInfo> databaseCopy = new List<CardInfo>(cardDatabase.GameCards); //Create a copy of the database to randomize cards to go to options of acquire new cards
        Debug.Log(databaseCopy);
        
        for(int i = 0; i < 3; i++) //Fill with 3 cards, one per button
        {
            int r = UnityEngine.Random.Range(0, databaseCopy.Count);
            CardInfo cardInfoInstance = UnityEngine.Object.Instantiate(databaseCopy[r]);
            cardOptions.Add(cardOptionsButtons[i], cardInfoInstance);
            cardOptionsButtons[i].image.sprite = cardInfoInstance.sprite;
            databaseCopy.RemoveAt(r);
        }
    }

    public void ClearCardOptions()
    {
        cardOptions.Clear();
        foreach(Button cardOptionButton in cardOptionsButtons)
        {
            cardOptionButton.image.sprite = null;
        }
    }

    public void ChooseCard0()
    {
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        if (cardOptions[cardOptionsButtons[0]] != null)
            playerDeck.cardsList.Add(cardOptions[cardOptionsButtons[0]]);
        else
            throw new Exception("there is no cardInfo attached to this button");
        cardOptions[cardOptionsButtons[0]] = null;
        StopSellection();
    }
    public void ChooseCard1()
    {
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        if (cardOptions[cardOptionsButtons[1]] != null)
            playerDeck.cardsList.Add(cardOptions[cardOptionsButtons[1]]);
        else
            throw new Exception("there is no cardInfo attached to this button");
        cardOptions[cardOptionsButtons[1]] = null;
        StopSellection();
    }
    public void ChooseCard2()
    {
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        if (cardOptions[cardOptionsButtons[2]] != null)
            playerDeck.cardsList.Add(cardOptions[cardOptionsButtons[2]]);
        else
            throw new Exception("there is no cardInfo attached to this button");
        cardOptions[cardOptionsButtons[2]] = null;
        StopSellection();
    }

    public void StopSellection()
    {
        CardSelectionUI.SetActive(false);// Deactivate the card selection UI
    }
    #endregion

}
