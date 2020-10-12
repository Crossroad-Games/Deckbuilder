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
    [SerializeField]private int PlayerHP; // Current HP
    [SerializeField]private int PlayerMaxHP;// Maximum HP
    [SerializeField]private int PlayerDefense;// Player Defense stat
    [SerializeField] private string Name;// Could be either a username or a preset name?
    #endregion
    [Space(5)]
    #region References
    [SerializeField] private Deck deck;
    [SerializeField] private Hand hand;
    [SerializeField] private CDPile cdPile;
    [SerializeField] private MouseOnEnemy enemyDetector;
    [SerializeField] private LayerMask cardLayer;
    [SerializeField] private LayerMask handZoneLayer;
    [SerializeField] private Button EndTurnButton;
    private TurnManager TurnMaster;
    #endregion


    #region Events
    public Action<GameObject> OnMouseEnterCard;
    public Action<GameObject> OnMouseExitCard;
    public Action<GameObject> OnCardSelected;
    public Action<GameObject> OnCardUnselected;
    public Action<GameObject> OnTargetCardUsed;
    #endregion

    #region Fields and Properties
    private GameObject previousHoverObject; //What mouse was pointing at when begin pointing somethingelse
    private Card SelectedCard; //Currently selected card
    private RaycastHit2D hitInfo; //What mouse is pointing at
    #endregion

    #region Booleans
    public bool isHoveringCard = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Initialization
        TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();
        OnMouseEnterCard += MouseStartedPointingToCard;
        OnMouseExitCard += MouseStoppedPointingToCard;
        isHoveringCard = false;
    }

    private void OnDisable()
    {
        OnMouseEnterCard -= MouseStartedPointingToCard;
        OnMouseExitCard -= MouseStoppedPointingToCard;
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

    public void FlipEndButton(bool Interactable)
    {
        EndTurnButton.interactable = Interactable;// Toggle the button
    }

    public void EndTurn()
    {
        if (TurnManager.State==CombatState.PlayerActionPhase)
            TurnMaster.EndPlayerTurn();
    }

    public void ProcessDamage(int Damage)
    {
        Damage = Damage - PlayerDefense;// Reduce the damage by the enemy defense
        Damage = Damage <= 0 ? 0 : Damage;// If the damage went beyond 0, set it to be 0, if not: keep the value
        LoseLife(Damage);// Apply damage to the enemy's HP
    }
    private void LoseLife(int Amount) => PlayerHP -= Amount;

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
        if(Input.GetMouseButtonDown(0))
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
        //Unselection ---------- Right now, whenever you release the mouse button, you unselect
        if(Input.GetMouseButtonUp(0))
        {

            EnemyClass enemy;
            if(enemyDetector.isMouseOnEnemy() != null)
            {
                enemy = enemyDetector.isMouseOnEnemy();
                TargetCard targetCard = (SelectedCard as TargetCard);
                if(targetCard != null)
                {
                    //TODO: targetCard.PerformAction(enemy);
                    /////// targetCard.Selected = false;
                    /////// Remove a CardPositionToFollow
                    /////// hand.SendCard(hand, CDPile);
                    /// Fazer a carta chamar a ação , mandar ela pra pilha de cooldown e atualizar a mão
                    Debug.Log("Perform Action , update hand and send card to CD Pile");
                    if (OnTargetCardUsed != null)
                    {
                        OnTargetCardUsed(targetCard.gameObject);
                    }
                    hand.SendCard(targetCard.cardInfo, cdPile); //Sends the card from hand to discard pile after calling the event of when the targetCard is used
                    SelectedCard.selected = false;
                    SelectedCard = null;
                }
            }
            else
            {
                if (SelectedCard != null)
                {
                    Card unselectedCard = SelectedCard;
                    SelectedCard.selected = false;
                    SelectedCard.followCardPositionToFollow = true;
                    if (SelectedCard != null)
                    {
                        OnCardUnselected(unselectedCard.gameObject);
                    }
                    SelectedCard = null;
                }
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
