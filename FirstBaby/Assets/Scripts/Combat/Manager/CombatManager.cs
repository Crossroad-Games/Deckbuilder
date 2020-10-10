using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region References
    private CombatPlayer combatPlayer;
    private Hand playerHandPile;
    private Deck playerDeck;
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        ///////////Initialization///////////////
        combatPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();
        playerHandPile = GameObject.FindGameObjectWithTag("Player").GetComponent<Hand>();
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        ///////////////////////////////////////

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   
}
