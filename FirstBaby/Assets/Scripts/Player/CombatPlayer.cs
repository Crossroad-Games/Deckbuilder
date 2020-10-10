using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CombatPlayer : MonoBehaviour
{
    [Header("Player Information")]
    #region Player Information
    [SerializeField]private int PlayerHP; // Current HP
    [SerializeField]private int PlayerMaxHP;// Maximum HP
    [SerializeField]private int PlayerDefense;// Player Defense stat
    #endregion
    [Space(5)]
    [SerializeField] private Deck deck;
    [SerializeField] private Hand hand;
    [SerializeField] private LayerMask cardLayer;
    private TurnManager TurnMaster;

    #region Events
    public Action<GameObject> OnMouseEnterCard;
    public Action<GameObject> OnMouseExitCard;
    #endregion

    #region Fields and Properties
    GameObject previousHoverObject;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();
        OnMouseEnterCard += MouseStartedPointingToCard;
        OnMouseExitCard += MouseStoppedPointingToCard;
    }

    private void OnDisable()
    {
        OnMouseEnterCard -= MouseStartedPointingToCard;
        OnMouseExitCard -= MouseStoppedPointingToCard;
    }

    // Update is called once per frame
    void Update()
    {
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
    #endregion
}
