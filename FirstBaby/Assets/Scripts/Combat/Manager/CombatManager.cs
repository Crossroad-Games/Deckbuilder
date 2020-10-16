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

    #region Booleans
    public bool Won = false;
    public bool Defeated = false;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        ///////////Reference Initialization///////////////
        combatPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();
        playerHandPile = GameObject.FindGameObjectWithTag("Player").GetComponent<Hand>();
        playerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();
        ///////////////////////////////////////
        ///////////Fields and Properties initialization///
        Won = false;
        Defeated = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   
}
