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
    [SerializeField]public PlayerData myData= new PlayerData();
    #endregion
    [Space(5)]
    #region References
    [SerializeField] private Deck deck=null;
    [SerializeField] private Hand hand=null;
    [SerializeField] private LayerMask cardLayer=0;
    [SerializeField] private Button EndTurnButton=null;
    private TurnManager TurnMaster;
    #endregion



    #region Events
    public Action<GameObject> OnMouseEnterCard;
    public Action<GameObject> OnMouseExitCard;
    #endregion

    #region Fields and Properties
    GameObject previousHoverObject;
    #endregion

    #region Player startup methods
    public void LoadSaveData() => myData = GameData.Current.PlayerData;
    private void Awake()
    {
        OnMouseEnterCard += MouseStartedPointingToCard;
        OnMouseExitCard += MouseStoppedPointingToCard;
        SaveLoad.LoadEvent += LoadSaveData;// Subscribes this method to the load event so that the player data is synced to the save file
    }
    void Start()
    {
        TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();
       
    }
    #endregion

    private void OnDisable()
    {
        OnMouseEnterCard -= MouseStartedPointingToCard;
        OnMouseExitCard -= MouseStoppedPointingToCard;
        SaveLoad.LoadEvent -= LoadSaveData;
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
    }
    #region Turn System
    public void FlipEndButton(bool Interactable)
    {
        EndTurnButton.interactable = Interactable;// Toggle the button
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
    private void LoseLife(int Amount) => myData.PlayerHP -= Amount;

    //------------------------------
    #region Mouse Hovering
    
    private void HoverMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hitInfo = Physics2D.Raycast(mousePos2D, Vector2.zero, 15f, cardLayer);

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
            CardSelection(current);
            //------------------------------------------
        }
        else // Now mouse is pointing at null ( not to any card )
        {
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

    private void CardSelection(GameObject card) //select and unselect a card.
    {
        if(Input.GetMouseButtonDown(0))
        {
            card.GetComponent<Card>().Selected = true;
            Debug.Log("Selecionada: " + card.GetComponent<Card>().Selected);
        }

        if(Input.GetMouseButtonUp(0))
        {
            card.GetComponent<Card>().Selected = false;
            card.GetComponent<Card>().followTarget = true; //TODO: When mouse up and not on available drop zone, go back to position in hand. If it is an attack card, need to have a different behaviour
            Debug.Log("selecionada: " + card.GetComponent<Card>().Selected);
        }
    }

    #endregion
}
