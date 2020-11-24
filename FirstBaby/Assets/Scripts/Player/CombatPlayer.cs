using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CombatPlayer : MonoBehaviour
{
    [Header("Player Information")]
    #region Player Information
    [SerializeField]public CombatPlayerData myData= new CombatPlayerData();
    [SerializeField] private TMP_Text ShieldAmount = null;// Text that display the amount of shield the player has
    [Range(0,Mathf.Infinity)][SerializeField] public float ShieldDecay=0.5f;// % by which the shield will decay every turn
    #endregion
    [Space(5)]
    #region References
    
    [SerializeField] private Deck deck=null;
    [SerializeField] private Hand hand=null;
    [SerializeField] private CDPile cdPile=null;
    public CDPile CdPile { get { return cdPile; } }
    [SerializeField] private MouseOnEnemy enemyDetector=null;
    [SerializeField] private LayerMask cardLayer=0;
    [SerializeField] private LayerMask handZoneLayer=0;
    [SerializeField] private Image HPBarFill=null;
    [SerializeField] private Image WardBarFill = null;
    [SerializeField] private TMP_Text HPBarValue = null;
    [SerializeField] private TMP_Text ArmorValue = null;
    [SerializeField] private GameObject WardOverloadAnchor = null;
    private List<Image> WardOverloadList = new List<Image>();
    private int InitialHP;// HP at the start of combat
    private Button EndTurnButton=null;
    private TurnManager TurnMaster;
    public CombatManager CombatManager;
    public Camera camera2;
    public Shield playerShield;
    private Bezier bezierArrowCurve;
    #endregion


    #region Events
    public Action<GameObject> OnMouseEnterCard;
    public Action<GameObject> OnMouseExitCard;
    public Action<GameObject> OnCardSelected;
    public Action<GameObject> OnCardUnselected;
    public Action<GameObject> OnTargetCardUsed;
    public Action<GameObject> OnNonTargetCardUsed;
    public Action<EnemyClass, int> OnPlayerProcessDamage;
    public Action OnPlayerSpendShield;
    public Action OnPlayerGainShield;
    #endregion

    #region Fields and Properties
    private GameObject previousHoverObject; //What mouse was pointing at when begin pointing somethingelse
    private PhysicalCard SelectedCard; //Currently selected card
    public RaycastHit2D hitInfo; //What mouse is pointing at
    public RaycastHit hitInfo3D;
    #endregion

    #region Booleans
    public bool isHoveringCard = false;
    public bool releasedMouseNotOnEnemy = false;
    public bool Disrupted = false;
    #endregion

    #region Player startup methods
    public void LoadSaveData()
    {
        myData = CombatGameData.Current.PlayerData;
        InitialHP = myData.PlayerLifeForce;// Sets the initial HP value when entering combat
        HPBarValue.text = myData.PlayerLifeForce + "/" + InitialHP;
    } 
    private void Awake()
    {
        //Initialization
        OnMouseEnterCard += MouseStartedPointingToCard;
        OnMouseExitCard += MouseStoppedPointingToCard;
        OnPlayerGainShield += UpdateShield;
        OnPlayerSpendShield += UpdateShield;
        playerShield = GetComponentInChildren<Shield>();
        UpdateShield();
        SaveLoad.LoadEvent += LoadSaveData;// Subscribes this method to the load event so that the player data is synced to the save file
        PauseGame.PauseEvent += FlipEndButton;
        isHoveringCard = false;
        releasedMouseNotOnEnemy = false;
        WardBarFill.fillAmount = 0;// Start with 0 shield
        WardOverloadList = WardOverloadAnchor.GetComponentsInChildren<Image>().ToList();// Converts all images on the children to a list of images
        foreach (Image Overload in WardOverloadList)// For each ward overload attached to the player
            if (Overload != null)// Check if null
                Overload.enabled = false;// Disable it
    }
    void Start()
    {
        TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();
        CombatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();
        EndTurnButton = GameObject.Find("Canvas").transform.Find("End Turn").GetComponent<Button>();
        bezierArrowCurve = GetComponent<Bezier>();
    }
    #endregion

    private void OnDisable()
    {
        OnMouseEnterCard -= MouseStartedPointingToCard;
        OnMouseExitCard -= MouseStoppedPointingToCard;
        OnPlayerGainShield -= UpdateShield;
        OnPlayerSpendShield -= UpdateShield;
        SaveLoad.LoadEvent -= LoadSaveData;
        PauseGame.PauseEvent -= FlipEndButton;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseGame.IsPaused)
            return;

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
    public void StartTurn()
    {
        SpendShield((int)Mathf.Ceil((myData.PlayerShield * (ShieldDecay))));
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
        OnPlayerSpendShield?.Invoke();    
        return CurrentShield;
    }
    public int LoseShield(int ShieldAmount)// Lose shield to some external effect
    {
        return SpendShield(ShieldAmount);// Lose this much shield and return the current amount of shield
    }
    public void GainShield(int ShieldAmount)// This function will modify the player's shield amount
    {
        // Any other methods that should be called when adding shield to the player's shield pool
        myData.PlayerShield += ShieldAmount;// Adds this amount to the player's shield pool
        OnPlayerGainShield?.Invoke();
    }
    public void ProcessDamage(EnemyClass attackingEnemy, int RawDamageTaken)
    {
        Debug.Log("Incoming "+RawDamageTaken+" Damage from: " + attackingEnemy);
        if (attackingEnemy != null)// If there is an attacking enemy
            OnPlayerProcessDamage?.Invoke(attackingEnemy, RawDamageTaken);
        RawDamageTaken -= myData.PlayerDefense;// Reduce the damage by the enemy defense
        RawDamageTaken -= SpendShield(RawDamageTaken);// Spend the shield pool to reduce the incoming damage
        RawDamageTaken = RawDamageTaken <= 0 ? 0 : RawDamageTaken;// If the damage went beyond 0, set it to be 0, if not: keep the value
        LoseLife(RawDamageTaken);// Apply damage to the enemy's HP
    }
    
    private void LoseLife(int Amount)
    {
        myData.PlayerLifeForce -= Amount;
        HPBarFill.fillAmount = ((float)myData.PlayerLifeForce / InitialHP) <= 1? ((float)myData.PlayerLifeForce / InitialHP) : 1;// Fill ball is based on the current amount of HP over initial amount of HP
        HPBarValue.text = myData.PlayerLifeForce + "/" + InitialHP;
        if (myData.PlayerLifeForce <=0)
        {
            Die();
        }
    }

    public void GainLife(int Amount)
    {
        myData.PlayerLifeForce += Amount;
        HPBarFill.fillAmount = ((float)myData.PlayerLifeForce / InitialHP) <= 1 ? ((float)myData.PlayerLifeForce / InitialHP) : 1;// Fill ball is based on the current amount of HP over initial amount of HP
        HPBarValue.text = myData.PlayerLifeForce + "/" + InitialHP;
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
    }

    private void MouseStoppedPointingToCard(GameObject card)
    {
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
                    PhysicalCard card = hitInfo.collider.gameObject.GetComponent<PhysicalCard>();
                    if (card.selectable)
                    {
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
        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        //Once Selected Card, be able to unselect Card or Select enemy
        else if(SelectedCard != null)
        {
            if(SelectedCard.type == "TargetCard" || SelectedCard.type == "ConcoctCardAttack" || SelectedCard.type == "ConcoctCardEffectTarget")
            {
                if(!releasedMouseNotOnEnemy) //if didn't release mouse not on enemy
                {
                    if (Input.GetMouseButtonUp(0)) // mouse release
                    {
                        if (enemyDetector.isMouseOnEnemy()) //Mouse over enemy -> enemy selected
                        {
                            SelectedCard.selected = false; //card no longer selected
                            //Send card to cdPile -> Update SelectedCard to null -> TODO:callAction
                            EnemyClass enemyToUseAction = enemyDetector.isMouseOnEnemy();
                            if (OnTargetCardUsed != null)
                            {
                                OnTargetCardUsed(SelectedCard.gameObject); //TargetCard used event
                            }
                            Debug.Log("Soltou mouse no inimigo: " + SelectedCard);
                            StartCoroutine(SelectedCard.GetComponent<PhysicalCard>().ExecuteAction(enemyToUseAction));
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
                            SelectedCard.selected = false; //card no longer selected
                            //Send card to cdPile -> Update SelectedCard to null -> callAction
                            EnemyClass enemyToUseAction = enemyDetector.isMouseOnEnemy();
                            if (OnTargetCardUsed != null)
                            {
                                OnTargetCardUsed(SelectedCard.gameObject); //TargetCard used event
                            }
                            Debug.Log("Apertou mouse no inimigo: " + SelectedCard);
                            StartCoroutine(SelectedCard.GetComponent<PhysicalCard>().ExecuteAction(enemyToUseAction));
                            SelectedCard = null; //Update selectedCard
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
            else if(SelectedCard.type == "NonTargetCard" || SelectedCard.type == "ConcoctCardDefense" || SelectedCard.type == "ConcoctCardEffectNonTarget")
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (IsMouseInHandZone()) // if release in hand zone do nothing (keep dragging)
                    {
                        return;
                    }
                    else //if release not in hand zone -> do action
                    {
                        SelectedCard.selected = false; //card no longer selected
                        if (OnNonTargetCardUsed != null)
                        {
                            OnNonTargetCardUsed(SelectedCard.gameObject); //Call event of when nonTargetCard is used
                        }
                        StartCoroutine(SelectedCard.ExecuteAction());
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
                        SelectedCard.selected = false; //card no longer selected
                        if (OnNonTargetCardUsed != null)
                        {
                            OnNonTargetCardUsed(SelectedCard.gameObject); //Call event of when nonTargetCard is used
                        }
                        StartCoroutine(SelectedCard.ExecuteAction());
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

    #region UI Update
    public void UpdateShield()
    {
        if (myData.PlayerShield >= 100)// If there is more than 100 Ward
        {
            WardBarFill.fillAmount = (myData.PlayerShield % 100) != 0 ? ((myData.PlayerShield % 100) / 100f) : 1;// Check if it is a multiple of 100, because 200 should be 100 on the bar and 1 Charge
            ShieldAmount.text = (myData.PlayerShield % 100) != 0 ? $"{(myData.PlayerShield % 100)}" : "100";// Uses the variable as a string
            playerShield.gameObject.SetActive(true);
            for (var iterator = 0; iterator < 9; iterator++)// Go through all ward overload charges
                WardOverloadList[iterator].enabled = iterator+1 <= (Mathf.FloorToInt(myData.PlayerShield / 100));//  Enable all below the threshold(multiples of 100) and disable all above it
        }
        else if(myData.PlayerShield > 0 && myData.PlayerShield < 100)
        {
            WardBarFill.fillAmount = (float)myData.PlayerShield / 100f;// If there is less than 100 shield, convert it directly to %
            ShieldAmount.text = $"{myData.PlayerShield}";// Uses the variable as a string
            playerShield.gameObject.SetActive(true);
            Color color = playerShield.shieldMaterial.GetColor("shieldColor");
            float intensity = ((float) myData.PlayerShield / 50) * 4;
            Debug.Log(intensity);
            float factor = Mathf.Pow(2, intensity);
            Color newColor = new Color(color.r * intensity, color.g * intensity, color.b * intensity);
            playerShield.shieldMaterial.SetColor("shieldColor", newColor);
            foreach (Image OverloadCharge in WardOverloadList)// Cycle through all overload charge sprites
                if (OverloadCharge != null)// If it is not null
                    OverloadCharge.enabled = false;// Disable it
        }
        else
        {
            playerShield.gameObject.SetActive(false);
        }
        
    }
    public void ArmorUpdate(int Amount)
    {
        myData.PlayerDefense += Amount;// Increases or decreases the player's Armor value
        ArmorValue.text= $"{myData.PlayerDefense}";// Uses the variable as a string
    }
    #endregion
}
