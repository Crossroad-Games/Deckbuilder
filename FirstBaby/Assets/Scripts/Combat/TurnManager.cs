using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public enum CombatState { CombatStart , PlayerStartTurn, PlayerActionPhase, PlayerEndTurn, EnemyPhaseStart, EnemyStartTurn, EnemyActionPhase, EnemyEndTurn, EnemyEndPhase}
public class TurnManager : MonoBehaviour
{
    #region Event Declarations
    public static Action CombatStart;
    public static Action PlayerTurnStart;
    public static Action PlayerTurnEnd;
    public static Action EnemyPhaseStart;
    public static Action EnemyPhaseEnd;
    public static Action EnemyStartTurn;
    public static Action EnemyEndTurn;
    #endregion
    
    [SerializeField]public int TurnCount=0;// Which turn this combat is currently at

    #region References
    private EnemyManager EnemyManager;
    private CombatManager combatManager;
    private Deck PlayerDeck;
    private CDPile PlayerCDPile;
    private Hand PlayerHand;
    #endregion
    private void Awake()
    {
        EnemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();// Reference to the enemy manager is stored
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();// Reference to the combat manager
        PlayerDeck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();// Reference to the deck
        PlayerCDPile= GameObject.FindGameObjectWithTag("Player").transform.Find("CDPile").GetComponent<CDPile>();// Reference to the CD Pile
        PlayerHand = GameObject.FindGameObjectWithTag("Player").GetComponent<Hand>();// Reference to the hand
        SaveLoad.LoadEvent += LoadState;// Subscribe to this event in order to load the turn state when the player loads their information
    }
    private void OnDisable()
    {
        SaveLoad.LoadEvent -= LoadState;// Unsubscribe to this event when disabled
    }
    
    #region Turn State Control
    [SerializeField] public static CombatState State { get; private set; }// Current combat state
    private int StateNumber = 0;
    public void NextState()// Go to the next state or the player turn start if the are no more states
    {
        if (!combatManager.Won && !combatManager.Defeated)// If the player hasn't won or lost
        {
            if (StateNumber < 8)// If the State is not the enemy end phase
                StateNumber++;// Go to the next state
            else
                StateNumber = 1;// Go to the player turn start
            State = (CombatState)StateNumber;// Updates the current combat state
        }
    }
    public void IncrementTurn() => TurnCount++;// Count the current turn
    public void LoadState()// Loads the turn state information to match that which was stored on the saved file
    {
        State = CombatGameData.Current.whichCombatState;
        StateNumber = (int) State;
        TurnCount = CombatGameData.Current.TurnCount;
        StateCall();// Call the events on the loaded state
    }
    private void StateCall()
    {
        switch (State)// Check which state the save file is currently at
        {
            case (CombatState)0:// Combat Start
                CombatStart?.Invoke();// Call all methods subscribed to the beginning of combat
                PlayerTurnStart?.Invoke();// Call all methods subscribed to the start of the player turn
                break;
            case (CombatState)1:// Player Turn Start
                PlayerTurnStart?.Invoke();
                break;
        }
    }
    #endregion

    public void EndPlayerTurn()// Updates the current game state to be the end of player turn
    {
        StartCoroutine(HoldPlayerEndTurn());
    }
    public void EndEnemyTurn(bool EndPhase)//  Updates the current game state to be either the next enemy or the player
    {
        StartCoroutine(HoldEnemyEndTurn(EndPhase));
    }

    #region Pause State Coroutines
    IEnumerator HoldPlayerEndTurn()// This coroutine will hold the game state until the user unpauses
    {
        while (PauseGame.IsPaused)
            yield return null;
        NextState();
        PlayerTurnEnd?.Invoke();// Invoke all methods subscribed to this event
        NextState();
        EnemyPhaseStart?.Invoke();// Invoke all methods subscribed to this event
        EnemyManager.StartEnemyPhase();// Call the method that will handle the setup for this phase
        yield break;
    }
    IEnumerator HoldEnemyEndTurn(bool EndPhase)// This coroutine will hold the game state until the user unpauses
    {
        while (PauseGame.IsPaused)
            yield return null;
        NextState();
        EnemyEndTurn?.Invoke();// Invoke all methods subscribed to this event
        if (EndPhase)// If there are no enemies left
        {
            NextState();
            EnemyPhaseEnd?.Invoke();// // Invoke all methods subscribed to this event
            PlayerTurnStart?.Invoke();// Execute all the methods that should be called when the Player's turn start
        }
        else
        {
            StateNumber -= 2;// Go back to the Enemy Start Turn
            State = (CombatState)StateNumber;// Selects the proper state
            EnemyManager.StartEnemyTurn();// Calls the method that will handle the start of the next enemy turn
        }
        yield break;
    }
    IEnumerator HoldForPause(Action PausedAction)// Coroutine that will stop the next event from happening until the game is unpaused
    {
        while(PauseGame.IsPaused)
            yield return null;
        PausedAction?.Invoke();
        yield break;
    }
    #endregion
}
