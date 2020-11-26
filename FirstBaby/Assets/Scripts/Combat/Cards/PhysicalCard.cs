using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicalCard : MonoBehaviour
{
    #region Reference
    public CombatPlayer Player;
    private CombatManager combatManager;
    public CardInfo cardInfo;
    public Hand playerHand;
    public EnemyClass TargetEnemy;
    public VirtualCard thisVirtualCard;
    [SerializeField] private CombatProperties combatProperties=null;
    public Dictionary<string, CardExtension> CardExtensions = new Dictionary<string, CardExtension>();
    public CombatProperties CombatProperties { get { return combatProperties; } }
    public List<GameObject> Tooltips;// List of which tooltips this card will show
    #endregion

    #region Booleans
    private bool Selected;
    public bool selected //when card is being selected
    {
        get { return Selected; }
        set 
        { 
            if(value)// When card is selected
                foreach (GameObject Tooltip in Tooltips)// Cycle through all tooltips on this card
                    if (Tooltip != null)// Check if null
                        Tooltip.SetActive(false);// Deactivate based on the value being passed
            Selected = value;
        }
    }
    public bool concocted = false; //When card is being concocted by another card
    public bool isConcoct = false; //Wether card is a concoct card or not
    public bool selectable = true; //Determines wether card is selectable or not
    private bool Highlighted;
    public bool highlighted// when card is being highlighted
    {
        get { return Highlighted; }
        set
        {
            foreach (GameObject Tooltip in Tooltips)// Cycle through all tooltips on this card
                if(Tooltip!=null)// Check if null
                    Tooltip.SetActive(value);// Activate or deactivate based on the value being passed
            Highlighted = value;
        }
    }
    
    public float highlightPreviousHeight; // When the card will be highlighted, stores the height of the card of when it wasn't highlighted
    public Quaternion highlightPreviousRotation; // When the card will be highlighted, stores the rotation of the card of when it wasn't highlighted
    public bool hoverEffectsEnabled = true; //wether hover effects are enabled for this card
    public bool beingDrawn; // when card is being drawn
    public bool returningToHand; // when card is returning to hand
    public bool beingHovered; // when card is being hovered by mouse
    protected bool canGotoCDPile = false; // Flag for when card finishes all of it's desired behaviours
    protected bool effectFinished = false; // Flag for when should call the Effect Finished callback ( finish card behaviour )
    protected bool dealDamageFinished = false; // Flag for when should call the dealDamage Finished callback ( go to card effect )
    protected bool gainShield_HealthFinished = false; // Flag for when should call the dealDamage Finished callback ( go to card effect )
    public bool doEffectWhenConcocted = true; // When card is being concocted, do it's effect or not
    #endregion

    public string type = "none";
    public CardPorpuse cardPorpuse;

    #region CardValues
    public int BaseDamage = 0;// Damage value that will be applied to the Enemy
    public int BaseShield = 0; //Value of shield to gain
    public int BaseHeal = 0; //Value to heal directly in life
    public int AddValue = 0, SubtractValue = 0;// Values that modify the base value
    public float Multiplier = 1, Divider = 1;// Values that multiply or divide the modified base value
    public int CardLevel = 0;
    private Vector3 TooltipFirstPosition = new Vector3(7,5.5f,0);
    #endregion

    public bool followCardPositionToFollow; //true if we want card to follow target

    public void ResetCardInfo()
    {
    selected = false; 
    concocted = false;
    isConcoct = false;
    selectable = true; 
    highlighted = false; 
    hoverEffectsEnabled = true;
    canGotoCDPile = false; 
    effectFinished = false;
    dealDamageFinished = false;
    gainShield_HealthFinished = false;
    doEffectWhenConcocted = true;
}

    protected virtual void Awake()
    {
        Tooltips = new List<GameObject>();
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();
        thisVirtualCard = GetComponent<VirtualCard>();
        

    }
    protected virtual void Update()
    {
        UpdateCardText();
    }
    public abstract void LevelRanks();
    protected abstract void UpdateCardText();// Method called to determine each card text component current text 
    #region Instantiate Keyword Tooltips
    protected virtual void TooltipListDefinition()// Method used to cycle through all the keyword tooltips
    {
        #region Extensions
        foreach (string keyword in CardExtensions.Keys)// Cycle through the physical keywords
            if (keyword != string.Empty)// Check if null
                InstantiateTooltip(keyword);// Instantiate a tooltip based on its keyword name
        foreach (string keyword in thisVirtualCard.virtualCardExtensions.Keys)// Cycle through the virtual keywords
            if (keyword != string.Empty)// Check if null
                InstantiateTooltip(keyword);// Instantiate a tooltip based on its keyword name
        #endregion
        if (cardPorpuse == CardPorpuse.Attack)// If this is an attack card
            InstantiateTooltip("Damage");// Add a Damage tooltip
        else if (cardPorpuse == CardPorpuse.Defense)// If this is a defense card
            InstantiateTooltip("Ward");// Add a Ward tooltip
    }
    protected void InstantiateTooltip(string keyword)// Instantiates a tooltip based on the string argument
    {
        GameObject TooltipObj = null;// Temp game object to handle position, parent, active status...
        TooltipObj = Instantiate(Resources.Load("UI/Tooltip/" + keyword + " Tooltip")) as GameObject;// Access the tooltip folder and acquire the appropriate gameobject 
        TooltipObj.transform.SetParent(this.transform);// Set this card as its parent
        if (Tooltips.Count != 0)
            TooltipFirstPosition.y -= TooltipObj.transform.Find("Box").localScale.y * 1.4f;// Move the next tooltip down based on this size
        else
        {

            Debug.Log("Tooltip name: "+keyword+"\n"+(TooltipObj.transform.Find("Box").localScale.y - .5f) * 1.4f);
            TooltipFirstPosition.y -= (TooltipObj.transform.Find("Box").localScale.y - .5f) * 1.4f;// Move the next tooltip down based on this size
        }
        TooltipObj.transform.localPosition = new Vector3(TooltipFirstPosition.x, TooltipFirstPosition.y, TooltipFirstPosition.z);// Move it to its position
        TooltipFirstPosition.y -= TooltipObj.transform.Find("Box").localScale.y*1.4f;// Move the next tooltip down based on this size
        Tooltips.Add(TooltipObj);// Add the tooltip object to the list
        TooltipObj.SetActive(false);// Disables all 
    }
    #endregion
    public virtual void Start()
    {
        foreach (CardExtension Keyword in GetComponents<CardExtension>())// For each Keyword attached to this card
            CardExtensions.Add(Keyword.Keyword, Keyword);// Store its reference
        TooltipListDefinition();
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * combatProperties.cardNormalScale;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();
        playerHand = Player.GetComponent<Hand>();
        highlighted = false;
        selectable = true;
        selected = false;
    }
    
    public virtual IEnumerator ExecuteAction()
    {
        if (CardExtensions.ContainsKey("Unstable"))// If this card has an Overflow effect
        {
            Debug.Log("Unstable");
            CardExtensions["Unstable"].ExtensionEffect();// Execute its overflow effect
        }
        if (CardExtensions.ContainsKey("Hasten"))// If this card has an Overflow effect
        {
            Debug.Log("Hasten");
            CardExtensions["Hasten"].ExtensionEffect();// Execute its overflow effect
        }
        Debug.Log("chamou executeAction coroutine");
        StartCoroutine(CardEffect());// Execute the card's effect
        yield return new WaitUntil(() => canGotoCDPile == true); //Suspends the coroutine execution until the canGoToCDPile flag is set to true
        //Send it to the CD pile
        if (!Player.CombatManager.Won && !Player.CombatManager.Defeated)
            playerHand.SendCard(this.gameObject, Player.CdPile); //Send cardInfo to CDPile
    }

    public virtual IEnumerator ExecuteAction(EnemyClass targetEnemy)
    {
        if (CardExtensions.ContainsKey("Unstable"))// If this card has an Overflow effect
        {
            Debug.Log("Unstable");
            CardExtensions["Unstable"].ExtensionEffect();// Execute its overflow effect
        }
        if (CardExtensions.ContainsKey("Hasten"))// If this card has an Overflow effect
        {
            Debug.Log("Hasten");
            CardExtensions["Hasten"].ExtensionEffect();// Execute its overflow effect
        }
        this.TargetEnemy = targetEnemy;
        Debug.Log("chamou executeAction coroutine");
        StartCoroutine(CardEffect()); // Execute the card's effect
        yield return new WaitUntil(() => canGotoCDPile == true); //Suspends the coroutine execution until the supplied delegate evaluates to true
        //Send it to the CD pile
        Debug.Log("vai mandar pro cdPile");
        if (!Player.CombatManager.Won && !Player.CombatManager.Defeated)
            playerHand.SendCard(this.gameObject, Player.CdPile); //Send cardInfo to CDPile
    }

    public virtual void EndCardEffect()
    {
        canGotoCDPile = true;
    }
    public virtual void EndDealDamage()
    {
        dealDamageFinished = true;
    }
    public virtual void EndGainShield_Health()
    {
        canGotoCDPile = true;
    }

    public virtual IEnumerator CardEffect()  // This is the field used by the card to describe and execute its action
    {
        yield return new WaitUntil(() => effectFinished == true);
        EndCardEffect();
    }

    public virtual IEnumerator DealDamage()
    {
        Debug.Log(AddValue);
        TargetEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        //EndDealDamage();
        yield return new WaitUntil(() => dealDamageFinished == true);
    }

    public virtual IEnumerator GainShield_Health()
    {
        Player.GainShield((BaseShield + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        Player.GainLife((BaseHeal + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
        //EndGainShield_Health();
        yield return new WaitUntil(() => gainShield_HealthFinished == true);
    }

    public virtual void DealEffect()
    {

    }

    #region Utilities
    public void SetEffectFinishedTrue()
    {
        effectFinished = true;
    }
    #endregion
}
