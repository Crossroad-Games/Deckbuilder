using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Scene { Dungeon, Combat};
public enum CardPileToShow { DrawPile, CD, CD1, CD2, CD3plus, Hand};
public class CardsGallery : MonoBehaviour
{
    [SerializeField] Scene Scene;
    [SerializeField] private Vector3 TextPosition;
    public GameObject DeckGallery;
    public GameObject CombatTempGallery;
    public GameObject DrawPileUI;
    public GameObject CDUI;
    public Button ReturnButton;
    public Button drawPileButton;
    public Button CD_Button;
    public Button ShowCollection;
    private CombatPlayer combatPlayer;
    private DungeonPlayer dungeonPlayer;
    private Hand playerHand;
    private CDPile playerCDPile;
    private Deck playerDrawPile;
    public Font CopiesTextFont;

    #region Events
    public Action<GameObject> OnGalleryOpen;
    public Action<GameObject> OnGalleryClose;
    #endregion

    #region Temporary Lists of Cards
    private List<GameObject> cardsDisplayed = new List<GameObject>();
    public Dictionary<GameObject,Button> cardsDisplayedAsButtons = new Dictionary<GameObject, Button>();
    public List<GameObject> cardsSelected = new List<GameObject>();
    #endregion

    #region booleans
    [SerializeField]private bool isShowingGallery = false;
    #endregion

    CardCollectionComparer comparerInt = new CardCollectionComparer();
    CardGalleryComparer comparerCardGameobject = new CardGalleryComparer();
    CDGalleryComparer compareCD = new CDGalleryComparer();

    private void Awake()
    {
        #region Initialization
        if (Scene == Scene.Combat)// If this is a combat scene
        {
            combatPlayer = GameObject.Find("Combat Player").GetComponent<CombatPlayer>();
            playerHand = combatPlayer.GetComponent<Hand>();
            playerCDPile = combatPlayer.GetComponentInChildren<CDPile>();
            playerDrawPile = combatPlayer.GetComponent<Deck>();
            CombatTempGallery = GameObject.Find("Combat Temp Gallery");
            #region Buttons
            DrawPileUI = GameObject.Find("Draw Pile");
            drawPileButton = GameObject.Find("Draw Pile").GetComponent<Button>();
            //CD UI buttons
            CDUI = GameObject.Find("CD");
            CD_Button = CDUI.GetComponent<Button>();
            #endregion
            //------------
            
            //---------------
            
            CombatTempGallery.SetActive(false);
            #endregion
        }
        else
        {
            dungeonPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();// Reference to the dungeon player is set
            ShowCollection = GameObject.Find("Menu").transform.Find("ShowCollectionButton").GetComponent<Button>();// Button that will show the card collection
        }
        DeckGallery = GameObject.Find("Deck Gallery");
        ReturnButton = GameObject.Find("Return").GetComponent<Button>();
        ReturnButton.gameObject.SetActive(false);
        DeckGallery.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ui manager start");
        if (Scene == Scene.Combat)
        {
            drawPileButton.onClick.AddListener(delegate { ShowTempGallery(playerDrawPile.cardsList,true,false); });
            CD_Button.onClick.AddListener(delegate { ShowTempGallery(playerCDPile.cardsList,false,true); });
            drawPileButton.onClick.AddListener(delegate { AddReturnFunction(drawPileButton, CD_Button); });// Button will also act as a return button
            CD_Button.onClick.AddListener(delegate { AddReturnFunction(CD_Button, drawPileButton); });// Button will also act as a return button
        }
        else
        {
            ShowCollection.onClick.AddListener(delegate { ShowDeckGallery(dungeonPlayer.myData.CardCollectionID); });
        }
    }
    public void AddReturnFunction(Button thisButton, Button otherButton)
    {
        thisButton.onClick.RemoveAllListeners();// Remove listeners when this button is clicked
        thisButton.onClick.AddListener(delegate { CloseDrawPileGallery(thisButton, otherButton); });// When the button is clicked again, close the gallery
        otherButton.interactable = false;// Does not allow the user to interact with the other gallery button
    }
    public void ShowDeckGallery(List<int> cardsToShow)
    {
        if (!isShowingGallery)
        {
            // Show cards UI
            isShowingGallery = true;
            if(Scene==Scene.Combat)
            {
                if (OnGalleryOpen != null)
                {
                    foreach (PhysicalCard card in playerHand.physicalCardsInHand)
                    {
                        OnGalleryOpen(card.gameObject);
                    }
                }
                else
                    throw new Exception("nothing on OnGalleryOpen");
            }
            DeckGallery.SetActive(true);
            cardsToShow.Sort(comparerInt);
            var iterator = 0;
            var Sequence = 0;// How many cards are equal to each other
            Text newText = null;
            GameObject tempCard = null;
            foreach (int card in cardsToShow)
            {      
                if (iterator!=0)// If it is not the first card
                    if(cardsToShow[iterator-1]!=card)// If the current card is different to the previous
                    {
                        Sequence = 1;// Not equal to the previous, restart counting
                        tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card), DeckGallery.transform.GetChild(0));
                        tempCard.name = $"{card}";
                        cardsDisplayed.Add(tempCard);
                        cardsDisplayedAsButtons.Add(tempCard, tempCard.GetComponent<Button>());
                        Debug.Log("instanciou card UI");
                    }
                    else// If the current card is equal to the previous
                    {
                        Sequence++;// Counting up all equal cards
                        if(tempCard.transform.Find("Amount of Copies")==null)// If there is no text yet
                        {
                            var tempObj = new GameObject("Amount of Copies");// Creates an empty game object
                            tempObj.transform.SetParent(tempCard.transform);// Set it as a child of the card
                            tempObj.transform.position = TextPosition;// Aligns the text with the card
                            tempObj.AddComponent<Text>();// Creates a text component
                            newText = tempObj.GetComponent<Text>();// Defines the reference to the text
                            newText.font = CopiesTextFont;// Sets the appropriate font
                        }    
                        newText.text = $"x{Sequence}";// Shows a text to indicate how many copies of that card are currently on the collection
                    }
                else
                {
                    Sequence = 1;// Not equal to the previous, restart counting
                    tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card), DeckGallery.transform.GetChild(0));
                    tempCard.name = $"{card}";
                    cardsDisplayed.Add(tempCard);
                    cardsDisplayedAsButtons.Add(tempCard, tempCard.GetComponent<Button>());
                    Debug.Log("instanciou card UI");
                }  
                iterator++;
            }
            //--------------------------------------//
        }
    }
    public void ShowTempGallery(List<GameObject> cardsToShow, bool SortbyID, bool SortbyCD)
    {
        if (!isShowingGallery)
        {
            // Show cards UI
            isShowingGallery = true;
            if (OnGalleryOpen != null)
            {
                foreach (PhysicalCard card in playerHand.physicalCardsInHand)
                {
                    OnGalleryOpen(card.gameObject);
                }
            }
            else
                throw new Exception("nothing on OnGalleryOpen");
            CombatTempGallery.SetActive(true);
            foreach (GameObject card in cardsToShow)
            {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID), CombatTempGallery.transform.GetChild(0));
                tempCard.name = Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID).name;

                tempCard.GetComponent<InfoHolderCardUI>().CurrentCardCD = card.GetComponent<VirtualCard>().CurrentCooldownTime;// Stores the card's current cd on the cardui info holder script
                cardsDisplayed.Add(tempCard);
                if (SortbyID)
                    cardsDisplayed.Sort(comparerCardGameobject);
                if(SortbyCD)
                    cardsDisplayed.Sort(compareCD);
                tempCard.transform.SetSiblingIndex(cardsDisplayed.IndexOf(tempCard));
                cardsDisplayedAsButtons.Add(tempCard,tempCard.GetComponent<Button>());
                Debug.Log("instanciou card UI");
            }
            playerHand.DisableCards();
            //--------------------------------------//
        }
    }

    public void ShowTempGallery(List<GameObject> cardsToShow, bool combatSelection)
    {
        if (!isShowingGallery)
        {
            //Show cards UI
            isShowingGallery = true;
            if (OnGalleryOpen != null)
            {
                foreach (PhysicalCard card in playerHand.physicalCardsInHand)
                {
                    OnGalleryOpen(card.gameObject);
                }
            }
            CombatTempGallery.SetActive(true);
            foreach (GameObject card in cardsToShow)
            {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID), CombatTempGallery.transform.GetChild(0));
                tempCard.name = Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID).name;
                cardsDisplayed.Add(tempCard);
                cardsDisplayed.Sort(comparerCardGameobject);
                tempCard.transform.SetSiblingIndex(cardsDisplayed.IndexOf(tempCard));
                cardsDisplayedAsButtons.Add(tempCard,tempCard.GetComponent<Button>());
                Debug.Log("instanciou card UI");
            }
            playerHand.DisableCards();
            //-------------------------------------//
            if (combatSelection)
                ActivateCombatSelection();
        }
    }

    public void ShowTempGallery(List<GameObject> cardsToShow, bool combatSelection, int selectionAmount)
    {
        if (!isShowingGallery)
        {
            isShowingGallery = true;
            //Show cards UI
            if (OnGalleryOpen != null)
            {
                foreach (PhysicalCard card in playerHand.physicalCardsInHand)
                {
                    OnGalleryOpen(card.gameObject);
                }
            }
            CombatTempGallery.SetActive(true);
            foreach (GameObject card in cardsToShow)
            {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID), CombatTempGallery.transform.GetChild(0));
                tempCard.name = Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID).name;
                cardsDisplayed.Add(tempCard);
                cardsDisplayed.Sort(comparerCardGameobject);
                tempCard.transform.SetSiblingIndex(cardsDisplayed.IndexOf(tempCard));
                cardsDisplayedAsButtons.Add(tempCard,tempCard.GetComponent<Button>());
                Debug.Log("instanciou card UI");
            }
            playerHand.DisableCards();
            //-------------------------------------//
            if (combatSelection)
                ActivateCombatSelection(selectionAmount);
        }
    }

    public void CloseDrawPileGallery(Button thisButton, Button otherButton)
    {
        if (isShowingGallery)
        {
            isShowingGallery = false;
            for (int i = cardsDisplayed.Count - 1; i >= 0; i--)
            {
                Destroy(cardsDisplayed[i]);
                cardsDisplayed.RemoveAt(i);
            }
            CombatTempGallery.SetActive(false);
            foreach (PhysicalCard card in playerHand.physicalCardsInHand)
            {
                OnGalleryClose(card.gameObject);
            }
            playerHand.EnableCards();
            thisButton.onClick.RemoveAllListeners();
            #region Reassign Listeners
            if (thisButton==CD_Button)
            {
                CD_Button.onClick.AddListener(delegate { ShowTempGallery(playerCDPile.cardsList, false, true); });
                CD_Button.onClick.AddListener(delegate { AddReturnFunction(CD_Button, drawPileButton); });
            }
            else if(thisButton==drawPileButton)
            {
                drawPileButton.onClick.AddListener(delegate { ShowTempGallery(playerDrawPile.cardsList, true, false); });
                drawPileButton.onClick.AddListener(delegate { AddReturnFunction(drawPileButton, CD_Button); });
            }
            otherButton.interactable = true;// Allows the user to interact with the other gallery buttons
            #endregion
        }
    }
    public void CloseCollectionGallery()
    {

        if (isShowingGallery)
        {
            isShowingGallery = false;
            for (int i = cardsDisplayed.Count - 1; i >= 0; i--)
            {
                Destroy(cardsDisplayed[i]);
                cardsDisplayed.RemoveAt(i);
            }
            DeckGallery.SetActive(false);
            if(Scene==Scene.Combat)
            {
                foreach (PhysicalCard card in playerHand.physicalCardsInHand)
                {
                    OnGalleryClose?.Invoke(card.gameObject);
                }
                playerHand.EnableCards();
            }  
            ReturnButton.onClick.RemoveAllListeners();
            ReturnButton.gameObject.SetActive(false);
        }
    }
    private void ActivateCombatSelection()
    {
        if(cardsDisplayed.Count > 0)
        {
            foreach(KeyValuePair<GameObject,Button> cardButton in cardsDisplayedAsButtons)
            {
                cardButton.Value.onClick.AddListener(delegate { CardSelection(cardButton.Key); });
            }
        }
    }

    private void ActivateCombatSelection(int AmountToSelect)
    {
        if (cardsDisplayed.Count > 0)
        {
            foreach (KeyValuePair<GameObject, Button> cardButton in cardsDisplayedAsButtons)
            {
                cardButton.Value.onClick.AddListener(delegate { CardSelection(cardButton.Key.gameObject, AmountToSelect); });
            }
        }
    }

    public void CardSelection(GameObject cardToBeSelected)
    {
        if (!cardToBeSelected.GetComponent<CardUI>().cardUISelected)
        {
            cardsSelected.Add(cardToBeSelected);
            cardToBeSelected.GetComponent<CardUI>().cardUISelected = true;
        }
        else
        {
            cardsSelected.Remove(cardToBeSelected);
            cardToBeSelected.GetComponent<CardUI>().cardUISelected = false;
        }
    }

    public void CardSelection(GameObject cardToBeSelected, int AmountToSelect)
    {
        if (!cardToBeSelected.GetComponent<CardUI>().cardUISelected && cardsSelected.Count < AmountToSelect)
        {
            Debug.Log(cardsSelected.Count);
            cardsSelected.Add(cardToBeSelected);
            cardToBeSelected.GetComponent<CardUI>().cardUISelected = true;
        }
        else
        {
            cardsSelected.Remove(cardToBeSelected);
            cardToBeSelected.GetComponent<CardUI>().cardUISelected = false;
        }
    }
}


public class CardCollectionComparer : IComparer<int>
{
    public int Compare(int id1, int id2)
    {
        if(id1 > id2)
        {
            return 1;
        }
        else if(id1 < id2)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}

public class CardGalleryComparer : IComparer<GameObject>
{
    public int Compare(GameObject card1, GameObject card2)
    {
        if (int.Parse(card1.name) > int.Parse(card2.name))
        {
            return 1;
        }
        else if (int.Parse(card1.name) < int.Parse(card2.name))
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
public class CDGalleryComparer : IComparer<GameObject>
{
    public int Compare(GameObject card1, GameObject card2)
    {
        var CD1 = card1.GetComponent<InfoHolderCardUI>().CurrentCardCD;
        var CD2 = card2.GetComponent<InfoHolderCardUI>().CurrentCardCD;
        Debug.Log("CD1 :"+CD1+"\n CD2: "+CD2);
        if (CD1 > CD2)
        {
            return 1;
        }
        else if (CD1 < CD2)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
