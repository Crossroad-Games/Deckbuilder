using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Scene { Dungeon, Combat};
public enum CardPileToShow { DrawPile, CD0, CD1, CD2, CD3plus, Hand};
public class CardsGallery : MonoBehaviour
{
    [SerializeField] Scene Scene; 
    public GameObject DeckGallery;
    public GameObject CombatTempGallery;
    public GameObject DrawPileUI;
    public GameObject CD0UI;
    public GameObject CD1UI;
    public GameObject CD2UI;
    public GameObject CD3plusUI;
    public Button ReturnButton;
    public Button drawPileButton;
    public Button CD0_Button;
    public Button CD1_Button;
    public Button CD2_Button;
    public Button CD3plus_Button;
    public Button ShowCollection;
    private CombatPlayer combatPlayer;
    private DungeonPlayer dungeonPlayer;
    private Hand playerHand;
    private CDPile playerCDPile;
    private Deck playerDrawPile;

    #region Events
    public Action<GameObject> OnGalleryOpen;
    public Action<GameObject> OnGalleryClose;
    #endregion

    #region Temporary Lists of Cards
    private List<GameObject> cardsDisplayed = new List<GameObject>();
    private List<Button> cardsDisplayedAsButtons = new List<Button>();
    public List<GameObject> cardsSelected = new List<GameObject>();
    #endregion

    #region booleans
    [SerializeField]private bool isShowingGallery = false;
    #endregion

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
            CD0UI = GameObject.Find("CD0");
            CD1UI = GameObject.Find("CD1");
            CD2UI = GameObject.Find("CD2");
            CD3plusUI = GameObject.Find("CD3+");
            CD0_Button = GameObject.Find("CD0").GetComponent<Button>();
            CD1_Button = GameObject.Find("CD1").GetComponent<Button>();
            CD2_Button = GameObject.Find("CD2").GetComponent<Button>();
            CD3plus_Button = GameObject.Find("CD3+").GetComponent<Button>();
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
            drawPileButton.onClick.AddListener(delegate { ShowTempGallery(playerDrawPile.cardsList); });
            CD0_Button.onClick.AddListener(delegate { ShowTempGallery(playerCDPile.CD0); });
            CD1_Button.onClick.AddListener(delegate { ShowTempGallery(playerCDPile.CD1); });
            CD2_Button.onClick.AddListener(delegate { ShowTempGallery(playerCDPile.CD2); });
            CD3plus_Button.onClick.AddListener(delegate { ShowTempGallery(playerCDPile.CD3plus); });
        }
        else
        {
            ShowCollection.onClick.AddListener(delegate { ShowTempGallery(dungeonPlayer.myData.CardCollectionID); });
        }
    }

    public void ShowTempGallery(List<int> cardsToShow)
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
            ReturnButton.gameObject.SetActive(true);
            ReturnButton.onClick.AddListener(CloseCollectionGallery);
            foreach (int card in cardsToShow)
            {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card), DeckGallery.transform.GetChild(0));
                cardsDisplayed.Add(tempCard);
                cardsDisplayedAsButtons.Add(tempCard.GetComponent<Button>());
                Debug.Log("instanciou card UI");
            }
            //--------------------------------------//
        }
    }
    public void ShowTempGallery(List<GameObject> cardsToShow)
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
            ReturnButton.gameObject.SetActive(true);
            ReturnButton.onClick.AddListener(CloseDrawPileGallery);
            foreach (GameObject card in cardsToShow)
            {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID), CombatTempGallery.transform.GetChild(0));
                cardsDisplayed.Add(tempCard);
                cardsDisplayedAsButtons.Add(tempCard.GetComponent<Button>());
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
            ReturnButton.gameObject.SetActive(true);
            ReturnButton.onClick.AddListener(CloseDrawPileGallery);
            foreach (GameObject card in cardsToShow)
            {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID), CombatTempGallery.transform.GetChild(0));
                cardsDisplayed.Add(tempCard);
                cardsDisplayedAsButtons.Add(tempCard.GetComponent<Button>());
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
            ReturnButton.gameObject.SetActive(true);
            ReturnButton.onClick.AddListener(CloseDrawPileGallery);
            foreach (GameObject card in cardsToShow)
            {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID), CombatTempGallery.transform.GetChild(0));
                cardsDisplayed.Add(tempCard);
                cardsDisplayedAsButtons.Add(tempCard.GetComponent<Button>());
                Debug.Log("instanciou card UI");
            }
            playerHand.DisableCards();
            //-------------------------------------//
            if (combatSelection)
                ActivateCombatSelection(selectionAmount);
        }
    }

    public void CloseDrawPileGallery()
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
            ReturnButton.onClick.RemoveAllListeners();
            ReturnButton.gameObject.SetActive(false);
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
            foreach(Button cardButton in cardsDisplayedAsButtons)
            {
                cardButton.onClick.AddListener(delegate { CardSelection(cardButton.gameObject); });
            }
        }
    }

    private void ActivateCombatSelection(int AmountToSelect)
    {
        if (cardsDisplayed.Count > 0)
        {
            foreach (Button cardButton in cardsDisplayedAsButtons)
            {
                cardButton.onClick.AddListener(delegate { CardSelection(cardButton.gameObject, AmountToSelect); });
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
