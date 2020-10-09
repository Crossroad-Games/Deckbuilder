using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public enum CombatState { PlayerStartTurn, PlayerActionPhase, PlayerEndTurn, EnemyPhaseStart, EnemyStartTurn, EnemyActionPhase, EnemyEndTurn, EnemyEndPhase}
public class TurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    // Event List for each turn step and phase
    public static Action PlayerTurnStart;
    public static Action PlayerTurnEnd;
    public static Action EnemyPhaseStart;
    public static Action EnemyPhaseEnd;
    public static Action EnemyStartTurn;
    public static Action EnemyEndTurn;
    //
    [SerializeField]public static CombatState State { get; private set; }// Current combat state
    private int StateNumber=0;
    [SerializeField]private int TurnCount=0;
    void Start()
    {

        #region Event Subscriptions
        PlayerTurnStart += IncrementTurn;// Subscribe the turn count incrementation to be on the TurnStarts
        PlayerTurnStart += NextState;// Subscribe the method that will change the next state in line
        PlayerTurnEnd += NextState;// Subscribe the method that will change the next state in line
        EnemyPhaseStart += NextState;// Subscribe the method that will change the next state in line
        EnemyPhaseEnd += NextState;// Subscribe the method that will change the next state in line
        EnemyStartTurn += NextState;// Subscribe the method that will change the next state in line
        EnemyEndTurn += NextState;// Subscribe the method that will change the next state in line
        #endregion 
        State = (CombatState)StateNumber;
        PlayerTurnStart?.Invoke();// Execute all the methods that should be called when the Player's turn start

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void IncrementTurn() => TurnCount++;// Count the current turn
    private void NextState()
    {
        StateNumber++;
        State = (CombatState)StateNumber;
    }
    public void EndPlayerTurn()// Updates the current game state to be the end of player turn
    {
        NextState();
        PlayerTurnEnd?.Invoke();// Invoke all methods subscribed to this event
        Debug.Log("Current State:" + State);
    }
    public void EndEnemyTurn()
    {
        NextState();
        EnemyEndTurn?.Invoke();// Invoke all methods subscribed to this event
    }
}
