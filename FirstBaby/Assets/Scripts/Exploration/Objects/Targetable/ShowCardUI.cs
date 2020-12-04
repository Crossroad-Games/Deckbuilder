using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowCardUI : Targetable
{
    [SerializeField] private List<int> CardsIDtoShow;// List of all the cards this will show
    private CardDatabase CardData;// Reference will be used to determine which cards are going to be shown
    private Button NextCardButton;// Reference will be used to destroy this button after this is over
    private int whichCard=0;// Variable that controls which card is being shown
    private void Awake()
    {
        CardData = GameObject.Find("Game Master").GetComponent<CardDatabase>();// Reference is defined
        NextCardButton = GameObject.Find("Dungeon Canvas").transform.Find("Confirm Single Card Button").GetComponent<Button>();// Reference to the button is defined
        
    }
    public override void ExecuteAction()
    {
        CardData.ShowCard(CardsIDtoShow[whichCard]);// Show a card
        NextCardButton.gameObject.SetActive(true);// Turn on this game object
        NextCardButton.onClick.AddListener(delegate { NextCard(); });// Add a listener to this button to go to the next card when pressed
        whichCard++;// Prepare to show the next card
    }
    public void NextCard()
    {
        if (whichCard < CardsIDtoShow.Count)
        {
            CardData.ShowCard(CardsIDtoShow[whichCard]);// Show the next card
            whichCard++;// Prepare to show the next card
        }
        else// If there are no more cards to show
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().canMove = true;// Allow the player to move
            Destroy(CardData.CardUIGO);// Destroy the GO
            NextCardButton.gameObject.SetActive(false);// Turn off this game object
        }
    }
    private void OnDisable()
    {
        NextCardButton.onClick.RemoveListener(delegate { NextCard(); });// Remove listener from this button 
    }
}
