using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    #region Reference
    public CardInfo cardInfo;
    public Hand playerHand;
    [SerializeField] CombatProperties combatProperties;
    #endregion

    public bool selected;
    public string type = "none";

    public bool followCardPositionToFollow; //true if we want card to follow target


    

    void Start()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * combatProperties.cardNormalScale;
    }

    void Update()
    {
        
    }

    
}
