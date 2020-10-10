﻿using System.Collections;
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
    private EnemyManager EnemyManager;
    private int iterator;
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
        PlayerTurnStart?.Invoke();// Execute all the methods that should be called when the Player's turn start
        EnemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();// Reference to the enemy manager is stored
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void IncrementTurn() => TurnCount++;// Count the current turn
    private void NextState()
    {
        if (StateNumber < 7)
            StateNumber++;
        else
            StateNumber = 0;
        State = (CombatState)StateNumber;
    }
    public void EndPlayerTurn()// Updates the current game state to be the end of player turn
    {
        NextState();
        PlayerTurnEnd?.Invoke();// Invoke all methods subscribed to this event
        NextState();
        EnemyPhaseStart?.Invoke();// Invoke all methods subscribed to this event
        EnemyManager.StartEnemyPhase();// Call the method that will handle the setup for this phase
    }
    public void EndEnemyTurn(bool EndPhase)
    {
        NextState();
        EnemyEndTurn?.Invoke();// Invoke all methods subscribed to this event
        if (EndPhase)// If there are no enemies left
        {
            NextState();
            EnemyPhaseEnd?.Invoke();// // Invoke all methods subscribed to this event
            PlayerTurnStart?.Invoke();
        }
        else
        {
            Debug.Log("Here");
            StateNumber -= 2;// Go back to the Enemy Start Turn
            State = (CombatState)StateNumber;// Selects the proper state
            iterator++;
            if (iterator < 5)
                EnemyManager.StartEnemyTurn();// Calls the method that will handle the start of the next enemy turn
            else
                return;

        }
    }
}
