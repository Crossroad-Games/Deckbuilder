﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int MaxEnemies = 5;// Max number of enemies in the combat scene at the same time
    private int CurrentEnemyIndex = 0;// Start at the first enemy on the array
    private EnemyClass CurrentEnemyClass;// Reference to the enemy currently active
    [SerializeField] private EnemyClass[] CombatEnemies;// Array of enemies with their class reference
    private TurnManager TurnMaster;// Reference to the TurnManager script to access its methods
    private void Awake()
    {
        CombatEnemies = new EnemyClass[5];// Initialize the array of enemies to have a max size
        TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddEnemy(EnemyClass newEnemy)// Add a new enemy during the combat phase
    {
        var Iterator=0;// Variable used to count each loop and go through all array slots
        foreach(EnemyClass Enemy in CombatEnemies)// Go through all enemy slots available on the scene
        {
            if (Enemy == null)// Check if the slot is free
            {
                CombatEnemies[Iterator] = newEnemy;// Add this enemy to that slot
                return;// End this function
            }
            else
                Iterator++;// Increment the iterator
        }
    }
    
    public void StartEnemyPhase()
    {   
        CurrentEnemyIndex = 0;// Reset the index
        StartEnemyTurn();
    }
    public void StartEnemyTurn()
    {
        CurrentEnemyClass = CombatEnemies[CurrentEnemyIndex];// Start the enemy phase with the first enemy on the array
        CurrentEnemyClass.StartTurn();// Start the specific enemy's turn
        TurnManager.EnemyStartTurn?.Invoke();// Call all methods subscribed to the enemy's turn start
        CurrentEnemyClass.ActionPhase();// Call the method that controls the enemy's action phase
    }
    public void EndEnemyTurn()
    {
        if (CurrentEnemyIndex < 4)// If it is not the last possible enemy
        {
            if (CombatEnemies[CurrentEnemyIndex + 1] != null)// If there is another enemy that needs to do its turn logic
            {
                CurrentEnemyIndex++;// Cycle to the next enemy
                TurnMaster.EndEnemyTurn(false);// End this turn without ending the enemy phase
                
            }
            else
            {
                TurnMaster.EndEnemyTurn(true);// End this turn and the enemy phase
                CurrentEnemyClass = null;// Reset the current enemy
                CurrentEnemyIndex = 0;// Reset the index
            }
        }
        else
        {
            TurnMaster.EndEnemyTurn(true);// End this turn and the enemy phase
            CurrentEnemyClass = null;// Reset the current enemy
            CurrentEnemyIndex = 0;// Reset the index
        }
    }
}