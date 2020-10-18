using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CombatPlayer : MonoBehaviour
{
    [Header("Player Information")]
    #region Player Information
    [SerializeField]public CombatPlayerData myData= new CombatPlayerData();
    #endregion
    [Space(5)]
    #region References
    
    [SerializeField] private Deck deck=null;
    [SerializeField] private Hand hand=null;
    [SerializeField] private CDPile cdPile=null;
    [SerializeField] private MouseOnEnemy enemyDetector=null;
    [SerializeField] private LayerMask cardLayer=0;
    [SerializeField] private LayerMask handZoneLayer=0;
    private Button EndTurnButton=null;
    private TurnManager TurnMaster;
    private CombatManager CombatManager;
    #endregion


    #region Events
    public Action<GameObject> OnMouseEnterCard;
    public Action<GameObject> OnMouseExitCard;
    public Action<GameObject> OnCardSelected;
    public Action<GameObject> OnCardUnselected;
    public Action<GameObject> OnTargetCardUsed;
    public Action<GameObject> OnNonTargetCardUsed;
    #endregion

    #region Fields and Properties
    private GameObject previousHoverObject; //What mouse was pointing at when begin pointing somethingelse
    private Card SelectedCard; //Currently selected card
    private RaycastHit2D hitInfo; //What mouse is pointing at
    #endregion

    #region Booleans
    public bool isHoveringCard = false;
    public bool releasedMouseNotOnEnemy = false;
    #endregion

    #region Player startup methods
    public void LoadSaveData() => myData = CombatGameData.Current.PlayerData;
    private void Awake()
    {
        //Initialization
        OnMouseEnterCard += MouseStartedPointingToCard;
        OnMouseExitCard += MouseStoppedPointingToCard;
        SaveLoad.LoadEvent += LoadSaveData;// Subscribes this method to the load event so that the player data is synced to the save file
        PauseGame.PauseEvent += FlipEndButton;
        isHoveringCard = false;
        releasedMouseNotOnEnemy = false;
    }
    void Start()
    {
        TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();
        CombatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();
        EndTurnButton = GameObject.Find("Canvas").transform.Find("End Turn").GetComponent<Button>();
    }
    #endregion

    private void OnDisable()
    {
        OnMouseEnterCard -= MouseStartedPointingToCard;
        OnMouseExitCard -= MouseStoppedPointingToCard;
        SaveLoad.LoadEvent -= LoadSaveData;
        PauseGame.PauseEvent -= FlipEndButton;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseGame.IsPaused)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            deck.SendCard(deck.cardsList[0], hand); // This would be the draw, sending the first card of deck to hand
            Debug.Log("Chamou SendCard");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            deck.Shuffle(); //shuffles the Deck
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            hand.SendCard(hand.cardsList[0], deck);
        }

        //Mouse detection, what card is mouse hovering
        HoverMouse();
        //Select and Unselect a card
        CardSelection();
    }
    #region Turn System
    public void FlipEndButton(bool Interactable)
    {
        EndTurnButton.interactable = !Interactable;// Toggle the button
    }

    public void EndTurn()
    {
        if (TurnManager.State==CombatState.PlayerActionPhase)
            TurnMaster.EndPlayerTurn();
    }
    #endregion
    private int SpendShield(int Amount)// Spend an Amount of shield
    {
        if (Amount <= 0)// If whatever incoming amount is already below or equal than 0
            return 0;// Return 0, as no shield was spent
        int CurrentShield = myData.PlayerShield;// Current shield pool
        myData.PlayerShield -= Amount;// Reduce the pool by the amount of damage being applied
        myData.PlayerShield = myData.PlayerShield <= 0 ? 0 : myData.PlayerShield;// If the damage went beyond 0, set it to be 0, if not: keep the value
        return CurrentShield;
    }
    public void GainShield(int ShieldAmount)// This function will modify the player's shield amount
    {
        // Any other methods that should be called when adding shield to the player's shield pool
        myData.PlayerShield += ShieldAmount;// Adds this amount to the player's shield pool
    }
    public void ProcessDamage(int Damage)
    {
        Damage -= myData.PlayerDefense;// Reduce the damage by the enemy defense
        Damage -= SpendShield(Damage);// Spend the shield pool to reduce the incoming damage
        Damage = Damage <= 0 ? 0 : Damage;// If the damage went beyond 0, set it to be 0, if not: keep the value
        LoseLife(Damage);// Apply damage to the enemy's HP
    }
    
    private void LoseLife(int Amount)
    {
        myData.PlayerLifeForce -= Amount;
        if(myData.PlayerLifeForce <=0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameObject.Find("Combat Manager").GetComponent<CombatDefeat>().Defeat();
    }

    //------------------------------
    #region Mouse Hovering
    
    private void HoverMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        hitInfo = Physics2D.Raycast(mousePos2D, Vector2.zero, 15f, cardLayer);

        if(hitInfo.collider != null)
        {
            GameObject current = hitInfo.collider.gameObject;
            if (previousHoverObject != current) //if the mouse starts pointing into a card OR stops pointing into a card( goint another card )
            {
                if(previousHoverObject != null) //If the previous hovering object was another card
                {
                    //When the card stops being hovered
                    OnMouseExitCard(previousHoverObject);
                }
                //When card started being hovered
                OnMouseEnterCard(current);
                previousHoverObject = current;
            }
            //------------------------------------------
            //Here runs every frame mouse is over a card
            isHoveringCard = true;
            //CardSelection(current);
            //------------------------------------------
        }
        else // Now mouse is pointing at null ( not to any card )
        {
            isHoveringCard = false;
            if (previousHoverObject != null)
            {
                OnMouseExitCard(previousHoverObject);
                previousHoverObject = null;
            }
        }
    }

    private void MouseStartedPointingToCard(GameObject card)
    {
        Debug.Log("OnPointerEnter:  " + card.name);
    }

    private void MouseStoppedPointingToCard(GameObject card)
    {
        Debug.Log("OnPointerExit:  " + card.name);
    }

    private void CardSelection() //select and unselect a card.
    {
        //Selection
        if (SelectedCard == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hitInfo.collider != null && isHoveringCard) //If mouse is over a card when it is pressed
                {
                    Card card = hitInfo.collider.gameObject.GetComponent<Card>();
                    card.selected = true;
                    SelectedCard = card;
                    if (OnCardSelected != null)
                    {
                        OnCardSelected(card.gameObject);
                    }

                    Debug.Log("Selecionada: " + card.selected);
                }

            }
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //Once Selected Card, be able to unselect Card or Select enemy
        else if(SelectedCard != null)
        {
            if(SelectedCard.type == "TargetCard")
            {
                if(!releasedMouseNotOnEnemy) //if didn't release mouse not on enemy
                {
                    if (Input.GetMouseButtonUp(0)) // mouse release
                    {
                        if (enemyDetector.isMouseOnEnemy()) //Mouse over enemy -> enemy selected
                        {
                            //Send card to cdPile -> Update SelectedCard to null -> TODO:callAction
                            EnemyClass enemyToUseAction = enemyDetector.isMouseOnEnemy();
                            if (OnTargetCardUsed != null)
                            {
                                OnTargetCardUsed(SelectedCard.gameObject); //TargetCard used event
                            }
                            Debug.Log("Soltou mouse no inimigo: " + SelectedCard);
                            SelectedCard.GetComponent<Card>().ExecuteAction(enemyToUseAction);
                            if(!CombatManager.Won && !CombatManager.Defeated)
                                hand.SendCard(SelectedCard.cardInfo, cdPile);
                            
                            SelectedCard = null;
                        }
                        else // mouse released but not on any enemy
                        {
                            releasedMouseNotOnEnemy = true;
                        }
                    }
                }
                else if(releasedMouseNotOnEnemy) //if released mouse but not on enemy, begin checking if player will click the enemy
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        if(enemyDetector.isMouseOnEnemy())
                        {
                            //Send card to cdPile -> Update SelectedCard to null -> callAction
                            EnemyClass enemyToUseAction = enemyDetector.isMouseOnEnemy();
                            if (OnTargetCardUsed != null)
                            {
                                OnTargetCardUsed(SelectedCard.gameObject); //TargetCard used event
                            }
                            Debug.Log("Apertou mouse no inimigo: " + SelectedCard);
                            SelectedCard.GetComponent<Card>().ExecuteAction(enemyToUseAction);
                            if (!CombatManager.Won && !CombatManager.Defeated)
                                hand.SendCard(SelectedCard.cardInfo, cdPile); //Send cardInfo to CDPile
                            SelectedCard = null; //Update selectedCard
                            Debug.Log("Call TargetCard Action here"); //Here will go the action call
                        }
                        else
                        {
                            return;
                        }
                    }
                    //Now the Unselection of the card option
                    else if(Input.GetMouseButtonDown(1))
                    {
                        SelectedCard.selected = false; //card no longer selected
                        SelectedCard.followCardPositionToFollow = true; //Card should return to it's spot in hand
                        if(OnCardUnselected != null)
                        {
                            OnCardUnselected(SelectedCard.gameObject); //Event of when card is unselected
                        }
                        SelectedCard = null;
                        releasedMouseNotOnEnemy = false;
                    }
                }
                

            }
            else if(SelectedCard.type == "NonTargetCard")
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (IsMouseInHandZone()) // if release in hand zone do nothing (keep dragging)
                    {
                        return;
                    }
                    else //if release not in hand zone -> do action
                    {
                        if (OnNonTargetCardUsed != null)
                        {
                            OnNonTargetCardUsed(SelectedCard.gameObject); //Call event of when nonTargetCard is used
                        }
                        SelectedCard.ExecuteAction();
                        if (!CombatManager.Won && !CombatManager.Defeated)
                            hand.SendCard(SelectedCard.cardInfo, cdPile); //Send cardInfo to CDPile
                        SelectedCard = null; //update selected card to null
                    }
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    if (IsMouseInHandZone()) // if click in hand zone with card being dragged -> unselect card
                    {
                        SelectedCard.selected = false; //card no longer selected
                        SelectedCard.followCardPositionToFollow = true; //Card should return to it's spot in hand
                        if (OnCardUnselected != null)
                        {
                            OnCardUnselected(SelectedCard.gameObject); //Event of when card is unselected
                        }
                        SelectedCard = null;
                    }
                    else //if click not in hand zone -> do action
                    {
                        if (OnNonTargetCardUsed != null)
                        {
                            OnNonTargetCardUsed(SelectedCard.gameObject); //Call event of when nonTargetCard is used
                        }
                        SelectedCard.ExecuteAction();
                        if (!CombatManager.Won && !CombatManager.Defeated)
                            hand.SendCard(SelectedCard.cardInfo, cdPile); //Send cardInfo to CDPile
                        SelectedCard = null; //update selected card to null
                        Debug.Log("Call NonTargetCard Action here");
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (IsMouseInHandZone()) // if click with right mouse button in hand zone with card being dragged -> unselect card
                    {
                        SelectedCard.selected = false; //card no longer selected
                        SelectedCard.followCardPositionToFollow = true; //Card should return to it's spot in hand
                        if (OnCardUnselected != null)
                        {
                            OnCardUnselected(SelectedCard.gameObject); //Event of when card is unselected
                        }
                        SelectedCard = null;
                    }
                    else
                    {
                        SelectedCard.selected = false; //card no longer selected
                        SelectedCard.followCardPositionToFollow = true; //Card should return to it's spot in hand
                        if (OnCardUnselected != null)
                        {
                            OnCardUnselected(SelectedCard.gameObject); //Event of when card is unselected
                        }
                        SelectedCard = null;
                    }
                }
            }
            else
            {
                return;
            }
        }
    }

    public bool IsMouseInHandZone()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        hitInfo = Physics2D.Raycast(mousePos2D, Vector2.zero, 15f, handZoneLayer);
        if (hitInfo.collider != null) //if mouse is over hand zone
        {
            return true;
        }
        return false;
    }

    #endregion
}
