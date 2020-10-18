using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSubscriptions : MonoBehaviour
{
    #region Fields and Properties
    private EnemyManager EnemyManager;
    private CombatManager combatManager;
    private Deck PlayerDeck;
    private CDPile PlayerCDPile;
    private Hand PlayerHand;
    private TurnManager TurnMaster;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Ocorreu o Awake");
        #region References
        EnemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();// Reference to the enemy manager is stored
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();// Reference to the combat manager
        PlayerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();// Reference to the deck
        PlayerCDPile = GameObject.FindGameObjectWithTag("Player").transform.Find("CDPile").GetComponent<CDPile>();// Reference to the CD Pile
        PlayerHand = GameObject.FindGameObjectWithTag("Player").GetComponent<Hand>();// Reference to the hand
        TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();// Reference to the Turnmanager script is defined
        #endregion
        #region Subscriptions
        TurnManager.CombatStart += PlayerDeck.LoadDeck;// Subscribe to this event to load the save file deck information into this list
        TurnManager.CombatStart += PlayerDeck.Shuffle;// Subscribe to this event to shuffle the deck at the start of combat
        TurnManager.CombatStart += TurnMaster.NextState;// Subscribe the method that will change the next state in line
        TurnManager.PlayerTurnStart += TurnMaster.IncrementTurn;// Subscribe the turn count incrementation to be on the TurnStarts
        TurnManager.PlayerTurnStart += PlayerHand.DrawHand;
        TurnManager.PlayerTurnStart += PlayerCDPile.SendCardsBackToDeckAndShuffle;
        TurnManager.PlayerTurnStart += TurnMaster.NextState;// Subscribe the method that will change the next state in line
        TurnManager.PlayerTurnEnd += PlayerCDPile.UpdateCooldown;
        TurnManager.EnemyPhaseStart += TurnMaster.NextState;// Subscribe the method that will change the next state in line
        TurnManager.EnemyPhaseEnd += TurnMaster.NextState;// Subscribe the method that will change the next state in line
        TurnManager.EnemyStartTurn += TurnMaster.NextState;// Subscribe the method that will change the next state in line
        #endregion
    }
}
