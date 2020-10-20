using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class EnemyManager : MonoBehaviour
{
    // Declaration of references, variables and constants
    #region Fields and Properties
    // Start is called before the first frame update
    private int MaxEnemies = 5;// Max number of enemies in the combat scene at the same time
    private int CurrentEnemyIndex = 0;// Start at the first enemy on the list
    private EnemyClass CurrentEnemyClass;// Reference to the enemy currently active
    private EnemyDatabase EnemyDatabase;// Reference to the enemy database will be used to load the enemy prefabs
    [SerializeField] public List<EnemyClass> CombatEnemies;// List of enemies with their class reference
    [SerializeField] public List<EnemyData> EnemyData;// List of enemy datas
    [SerializeField] public List<Vector3> EnemyPositions;// List of enemy positions based on how many enemies there are on scene
    [SerializeField] public List<Vector3> HPPositions;// List of enemy positions based on how many enemies there are on scene
    private TurnManager TurnMaster;// Reference to the TurnManager script to access its methods
    private CombatManager combatManager;// Reference to the combat manager
    #endregion

    private void Awake()
    {
        EnemyDatabase = GameObject.Find("Game Master").GetComponent<EnemyDatabase>();// Reference to the database is set
        CombatEnemies = new List<EnemyClass>();// Initialize the list of enemies to have a max size
        EnemyData = new List<EnemyData>();// Initialize the listof enemy data to have a max sizes
        TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();// Reference to the Turnmanager script is defined
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();// Reference to the combat manager
        SaveLoad.LoadEvent += LoadData;// Subscribes this method to the load event to sync the enemies' data to the data stored on the save file
    }
    private void OnDisable()
    {
        SaveLoad.LoadEvent -= LoadData;
    }

    // Methods that execute specific actions as the turn phases are completeds
    #region Turn System
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
        if(!combatManager.Defeated && !combatManager.Won) 
            if (CurrentEnemyIndex < CombatEnemies.Count-1)// If it is not the last enemy on the list
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
    #endregion  

    // Methods that execute specific actions regarding the enemies on the scene: Add, Remove and Load
    private void EnemyDefeatCheck()// Checks if the enemy side lost this battle
    {
        if (CombatEnemies.Count <= 0)
            GameObject.Find("Combat Manager").GetComponent<CombatVictory>().Victory();// Player victory method is called
        else
            Debug.Log("Number of enemies on the scene: " + CombatEnemies.Count);
    }
    #region Manage Enemies
    public void AddEnemy(EnemyClass newEnemy)// Add a new enemy during the combat phase
    {
        if(CombatEnemies.Count<MaxEnemies)// If the amount of enemies on the scene is less than the max value
        {
            CombatEnemies.Add(newEnemy);// Add a new enemy
            EnemyData.Add(newEnemy.myData);// Store its data
        }
    }
    public void RemoveEnemy(EnemyClass RemovedEnemy)// Remove this enemyclass from the list of enemies on the scene
    {
        CombatEnemies.Remove(RemovedEnemy);// Remove this enemy from the list of combat enemies
        EnemyData.Remove(RemovedEnemy.myData);// Remove this enemy data from the list of combat enemies
        EnemyDefeatCheck();// Checks if the condition for defeat was reached
    }
    public void LoadData()// Loads the Data stored on the save file into the enemies
    {
        EnemyData = CombatGameData.Current.EnemyData;// Loads the information from the save file
        GameObject EnemyToSpawn=null;// Creates a temporary gameobject to store the enemies as they are spawned
        foreach (EnemyData Data in EnemyData)// Go through the list of saved enemy data
            if (Data != null)// If data is not null
            {
                EnemyToSpawn = (GameObject)Instantiate(EnemyDatabase.Enemy[Data.ID]);// Instantiates the enemy
                CombatEnemies.Add(EnemyToSpawn.GetComponent<EnemyClass>());// Store its class in the list
                EnemyToSpawn.GetComponent<EnemyClass>().myData = Data;// Stores the saved data into the new enemy
                EnemyToSpawn.transform.position = EnemyPositions[Data.Position];// This enemy will be sent to position it was first spawned on
            }
    }
    #endregion
}
