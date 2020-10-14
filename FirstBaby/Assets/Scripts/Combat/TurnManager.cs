using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public enum CombatState { PlayerStartTurn, PlayerActionPhase, PlayerEndTurn, EnemyPhaseStart, EnemyStartTurn, EnemyActionPhase, EnemyEndTurn, EnemyEndPhase}
public class TurnManager : MonoBehaviour
{
    #region Event Declarations
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
    #endregion
    private void Awake()
    {
        SaveLoad.LoadEvent += LoadState;// Subscribe to this event in order to load the turn state when the player loads their information
    }
    private void OnDisable()
    {
        SaveLoad.LoadEvent -= LoadState;// Unsubscribe to this event when disabled
    }
    void Start()
    {

        #region Event Subscriptions
        PlayerTurnStart += IncrementTurn;// Subscribe the turn count incrementation to be on the TurnStarts
        PlayerTurnStart += NextState;// Subscribe the method that will change the next state in line
        EnemyPhaseStart += NextState;// Subscribe the method that will change the next state in line
        EnemyPhaseEnd += NextState;// Subscribe the method that will change the next state in line
        EnemyStartTurn += NextState;// Subscribe the method that will change the next state in line
        #endregion
        State = (CombatState)StateNumber;

        StartCoroutine(HoldForPause(PlayerTurnStart));// Execute all the methods that should be called when the Player's turn start
        EnemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();// Reference to the enemy manager is stored
    }
   

    #region Turn State Control
    [SerializeField] public static CombatState State { get; private set; }// Current combat state
    private int StateNumber = 0;
    private void NextState()
    {
        if (StateNumber < 7)
            StateNumber++;
        else
            StateNumber = 0;
        State = (CombatState)StateNumber;
    }
    private void IncrementTurn() => TurnCount++;// Count the current turn
    public void LoadState()// Loads the turn state information to match that which was stored on the saved file
    {
        State = GameData.Current.whichCombatState;
        TurnCount = GameData.Current.TurnCount;
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
