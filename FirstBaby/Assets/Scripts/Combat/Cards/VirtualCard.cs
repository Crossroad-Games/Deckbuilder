using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualCard : MonoBehaviour
{
    public CardInfo cardInfo;
    private PhysicalCard PhysicalCardBehaviour;
    private Collider2D cardCollider;
    private CardExtension cardExtension;
    private Renderer cardRenderer;
    private MeshRenderer cardDescription;

    public bool isVirtual = false;
    public bool isPhysical = false;
    
    [SerializeField] private int currentCooldownTime;  //Current cooldown time
    public int CurrentCooldownTime
    {
        get { return currentCooldownTime; }
        set { currentCooldownTime = value; }
    }

    private void Awake()
    {
        #region Initialization
        cardCollider = GetComponent<Collider2D>();
        PhysicalCardBehaviour = GetComponent<PhysicalCard>();
        cardExtension = GetComponent<CardExtension>();
        cardRenderer = GetComponent<Renderer>();
        cardDescription = GetComponentInChildren<MeshRenderer>();
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

    public void TurnVirtual()
    {
        if (cardCollider != null)
            cardCollider.enabled = false; // Disable card collider
        else
            throw new NullReferenceException("no card collider");
        if (cardDescription != null)
            cardDescription.enabled = false; // Disable card description
        else
            Debug.Log("no card description");
        if (PhysicalCardBehaviour != null)
            PhysicalCardBehaviour.enabled = false; // Disable card behaviour
        else
            throw new NullReferenceException("no card behaviour");
        if (cardExtension != null)
            cardExtension.enabled = false; // Disable card extension script
        else
            Debug.Log("no card extension");
        if (cardRenderer != null)
            cardRenderer.enabled = false; // Disable card behaviour
        else
            throw new NullReferenceException("no card renderer");
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
            cardDescription.enabled = true; // Enable card description
        else
            Debug.Log("no card description");
        if (PhysicalCardBehaviour != null)
            PhysicalCardBehaviour.enabled = true; // Enable card behaviour
        else
            throw new NullReferenceException("no card behaviour");
        if (cardExtension != null)
            cardExtension.enabled = true; // Enable card extension script
        else
            Debug.Log("no card extension");
        if (cardRenderer != null)
            cardRenderer.enabled = true; // Enable card behaviour
        else
            throw new NullReferenceException("no card renderer");
        isPhysical = true;
        isVirtual = false;
    }
}
