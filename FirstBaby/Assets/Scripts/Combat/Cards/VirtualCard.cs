using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VirtualCard : MonoBehaviour
{
    public CardInfo cardInfo;
    public PhysicalCard PhysicalCardBehaviour;
    private Collider2D cardCollider;
    public CombatPlayer Player;
    public List<CardExtension> cardExtensions= new List<CardExtension>();
    public Dictionary<string,VirtualCardExtension> virtualCardExtensions = new Dictionary<string, VirtualCardExtension>();
    private Renderer cardRenderer;
    private GameObject cardDescription;
    private GameObject cardCooldown;
    public bool isVirtual = false;
    public bool isPhysical = false;
    [HideInInspector]public TMP_Text CardName;
    [HideInInspector]public TMP_Text CardText;
    #region Virtual Card Value
    public int BaseDamage = 0;// Damage value that will be applied to the Enemy
    public int BaseShield = 0; //Value of shield to gain
    public int BaseHeal = 0; //Value to heal directly in life
    public int AddValue = 0, SubtractValue = 0;// Values that modify the base value
    public float Multiplier = 1, Divider = 1;// Values that multiply or divide the modified base value
    public int CardLevel = 0;
    #endregion
    [SerializeField] private int currentCooldownTime;  //Current cooldown time
    public int CurrentCooldownTime
    {
        get { return currentCooldownTime; }
        set { currentCooldownTime = value; }
    }
    public int CalculateAction(int ActionValue)
    {
        return ((ActionValue + AddValue - SubtractValue) * Mathf.CeilToInt(Multiplier / Divider));// Calculate how much damage/ward this effect is outputting
    }
    private void Awake()
    {
        #region Initialization
        cardCollider = GetComponent<Collider2D>();
        PhysicalCardBehaviour = GetComponent<PhysicalCard>();
        cardExtensions = GetComponents<CardExtension>().ToList();
        cardRenderer = GetComponent<Renderer>();
        cardDescription = transform.Find("Description").gameObject;// Reference to the child that holds both text and name
        cardCooldown = transform.Find("Cooldown").gameObject;
        CardName = cardDescription.transform.Find("Name").GetComponent<TMP_Text>();// Reference to the TMP component on the child that represents its name
        CardText = cardDescription.transform.Find("Text").GetComponent<TMP_Text>();// Reference to the TMP Component on the child that represents its text
        #endregion
    }
    private void Start()
    {
        foreach (VirtualCardExtension Keyword in GetComponents<VirtualCardExtension>())// For each virtual Keyword attached to this card
            virtualCardExtensions.Add(Keyword.Keyword, Keyword);// Store its reference
    }
    #region Turn Virtual/Physical
    public void TurnVirtual()
    {
        if (cardCollider != null)
            cardCollider.enabled = false; // Disable card collider
        else
            throw new NullReferenceException("no card collider");
        if (cardDescription != null)
            cardDescription.SetActive(false); // Disable card description
        if (cardCooldown != null)
            cardCooldown.SetActive(false); // Disable card coodlown
        if (PhysicalCardBehaviour != null)
            PhysicalCardBehaviour.enabled = false; // Disable card behaviour
        else
            throw new NullReferenceException("no card behaviour");
        if (cardExtensions.Count > 0)
            foreach (CardExtension Keyword in cardExtensions)// For each non virtual card extension
                Keyword.enabled = false;// Deactivate it
        if (virtualCardExtensions.Count > 0)
            foreach (string Key in virtualCardExtensions.Keys)// Cycle through all the keywords
                if(Key!=null)// IF its not null
                    virtualCardExtensions[Key].enabled = true;// Activate it
        if (cardRenderer != null)
            cardRenderer.enabled = false; // Disable card behaviour
        else
            throw new NullReferenceException("no card renderer");
        foreach (GameObject Tooltip in PhysicalCardBehaviour.Tooltips)//Cycle through all the tooltips
            if (Tooltip != null)// Check if null
                Tooltip.SetActive(false);// Deactivates the tooltip
        isPhysical = false;
        isVirtual = true;
    }

    public void TurnPhysical()
    {
        if (cardCollider != null)
            cardCollider.enabled = true; // Enable card collider
        else
            throw new NullReferenceException("no card collider");
        if (cardDescription != null)
            cardDescription.SetActive(true); // Enable card description
        if (cardCooldown != null)
            cardCooldown.SetActive(true); // Disable card coodlown
        if (PhysicalCardBehaviour != null)
            PhysicalCardBehaviour.enabled = true; // Enable card behaviour
        else
            throw new NullReferenceException("no card behaviour");
        if (cardExtensions.Count > 0)
            foreach (CardExtension Keyword in cardExtensions)// For each non virtual card extension
                Keyword.enabled = true;// Activate it
        if (virtualCardExtensions.Count > 0)
            foreach (string Key in virtualCardExtensions.Keys)// Cycle through all the keywords
                if (Key != null)// IF its not null
                    virtualCardExtensions[Key].enabled = false;// Deactivate it
        if (cardRenderer != null)
            cardRenderer.enabled = true; // Enable card behaviour
        else
            throw new NullReferenceException("no card renderer");
        isPhysical = true;
        isVirtual = false;
    }
    #endregion
}
