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
    public static CombatState State { get; private set; }// Current combat state
    private int StateNumber=0;
    [SerializeField]private int TurnCount=0;
    void Start()
    {
        PlayerTurnStart += IncrementTurn;// Subscribe the turn count incrementation to be on the TurnStarts
        PlayerTurnStart += NextState;// Subscribe the method that will change the next state in line
        UpdateState();// Sets the current combat state to be the player turn
        PlayerTurnStart?.Invoke();// Execute all the methods that should be called when the Player's turn start
        UpdateState();// Sets the current combat state to be the player action phase
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void IncrementTurn() => TurnCount++;// Count the current turn
    private void NextState() => StateNumber++;// When the state changes, it will change to this one
    private void UpdateState() => State = (CombatState)StateNumber;// Updates the current combat state to be this one
}
