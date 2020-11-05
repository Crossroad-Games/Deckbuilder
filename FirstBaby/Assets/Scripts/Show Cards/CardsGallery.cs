using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Scene { Dungeon, Combat};
public class CardsGallery : MonoBehaviour
{
    [SerializeField] Scene Scene; 
    public GameObject CardGallery;
    public GameObject DrawPileGallery;
    public GameObject DrawPileUI;
    public Button ReturnButton;
    private CombatPlayer combatPlayer;
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
        combatPlayer = GameObject.Find("Combat Player").GetComponent<CombatPlayer>();
        playerHand = combatPlayer.GetComponent<Hand>();
        playerCDPile = combatPlayer.GetComponentInChildren<CDPile>();
        playerDrawPile = combatPlayer.GetComponent<Deck>();
        CardGallery = GameObject.Find("Card Gallery");
        DrawPileGallery = GameObject.Find("Draw Pile Gallery");
        DrawPileUI = GameObject.Find("Draw Pile");
        ReturnButton = GameObject.Find("Return").GetComponent<Button>();
        ReturnButton.gameObject.SetActive(false);
        CardGallery.SetActive(false);
        DrawPileGallery.SetActive(false);
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDrawPileGallery()
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
            DrawPileGallery.SetActive(true);
            ReturnButton.gameObject.SetActive(true);
            ReturnButton.onClick.AddListener(CloseDrawPileGallery);
            foreach (GameObject card in playerDrawPile.cardsList)
            {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID), DrawPileGallery.transform.GetChild(0));
                cardsDisplayed.Add(tempCard);
                cardsDisplayedAsButtons.Add(tempCard.GetComponent<Button>());
                Debug.Log("instanciou card UI");
            }
            playerHand.DisableCards();
            //--------------------------------------//
        }
    }

    public void ShowDrawPileGallery(bool combatSelection)
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
            DrawPileGallery.SetActive(true);
            ReturnButton.gameObject.SetActive(true);
            ReturnButton.onClick.AddListener(CloseDrawPileGallery);
            foreach (GameObject card in playerDrawPile.cardsList)
            {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID), DrawPileGallery.transform.GetChild(0));
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

    public void ShowDrawPileGallery(bool combatSelection, int selectionAmount)
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
            DrawPileGallery.SetActive(true);
            ReturnButton.gameObject.SetActive(true);
            ReturnButton.onClick.AddListener(CloseDrawPileGallery);
            foreach (GameObject card in playerDrawPile.cardsList)
            {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("UI/Cards UI/" + card.GetComponent<VirtualCard>().cardInfo.ID), DrawPileGallery.transform.GetChild(0));
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
            DrawPileGallery.SetActive(false);
            foreach (PhysicalCard card in playerHand.physicalCardsInHand)
            {
                OnGalleryClose(card.gameObject);
            }
            playerHand.EnableCards();
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
