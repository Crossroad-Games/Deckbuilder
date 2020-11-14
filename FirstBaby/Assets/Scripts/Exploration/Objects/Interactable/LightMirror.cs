using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightMirror : Interactable
{
    private List<Button> ShownCards= new List<Button>();
    private CardsGallery Gallery;// Reference to the UI Manager's Gallery component will allow us to expose the cards currently on the player's possession to choose one to level up
    public override void Awake()
    {
        base.Awake();
        Gallery = GameObject.Find("UI Manager").GetComponent<CardsGallery>();// Reference is defined
    }
    public override void Actuated()
    {
        Player.GetComponent<PlayerMovement>().canMove = false; //Disable player movement
        Gallery.ShowDeckGallery(Player.GetComponent<DungeonPlayer>().myData.CardCollectionID);// Enables the gallery 
        foreach (GameObject Card in Gallery.cardsDisplayedAsButtons.Keys)// Cycle through each displayed card
            if (Card != null)// If not null
                ShownCards.Add(Gallery.cardsDisplayedAsButtons[Card]);// Access the button linked to this gameobject and save it
        foreach (Button ChooseButton in ShownCards)// Cycle through each acquired button
            if (ChooseButton != null)// If not null
            {
                var ID = -1;// Temporary int variable
                int.TryParse(ChooseButton.gameObject.name, out ID);// Transforms the UI name to an ID int
                ChooseButton.onClick.AddListener(delegate { ChosenCard(ID); });// Adds a listener to the card which will pass its name converted to an int as the 
            }
    }
    public void ChosenCard(int CardID)// Method that will handle the lvl up event
    {
        Player.GetComponent<DungeonPlayer>().myData.CardLevels[CardID]++;// Increases the LVL of this card
        Gallery.CloseCollectionGallery();// Close the gallery
        Player.GetComponent<PlayerMovement>().canMove = true; //Disable player movement
    }
    public override void LoadDungeonState()
    {
        
    }
}
