using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class SaveLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private string dataPath;// Where it will be saved to in PC
    private string PlayerName;// Name the player chose to its save
    private CombatPlayer CombatPlayer;// Reference to the Combat Player will be used to get the player's combat information
    private DungeonPlayer DungeonPlayer;// Reference to the Dungeon Player will be used to get the player's dungeon information
    private Hand Hand;// Will be used to access the list of all the cards in your hand
    private Deck Deck;// Will be used to access the list of all the cards in your deck
    public bool CombatScene=false, DungeonScene=false;// Booleans used to check if the player is either on a combat or dungeon scene 
    private EnemyManager EnemyManager;// Reference to the Enemy Manager will be used to store the enemy data
    private TurnManager TurnMaster;// Reference to the turn manager will be used to get the current combat turn and phase
    public static Action LoadEvent;// This event is called to execute all the actions waiting for the load to be done
    private void Start()
    {
        
        if (CombatScene)// If the player is on a Combat Scene
        {
            CombatPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();// Reference to the player is defined, Save function will pull the information from this script to save it on a json file
            Hand = GameObject.FindGameObjectWithTag("Player").GetComponent<Hand>();// Reference to the Hand script is defined, save function will pull the information from it to store the cards that are currently at hand
            Deck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();// Reference to the Hand script is defined, save function will pull the information from it to store the cards that are currently at deck
            EnemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();// Reference to the enemy manager is defined, Save function will pull the enemy data stored
            TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();// Reference to the turn manager is defined, save function will pull the turn count and combat state to store it on the save file
        }
        else if(DungeonScene)// If the player is on Dungeon Scene
        {
           DungeonPlayer= GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();// Reference to the player is defined
        }
        Load();// Load the save files
    }
    private void SaveDungeon()// Method called to save the player's dungeon information
    {
        dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name")+".Dungeon");// Saves the information at this location
        DungeonGameData.Current.PlayerPosition = DungeonPlayer.transform.position;// Stores the player's position 
        DungeonGameData.Current.PlayerData = DungeonPlayer.myData;// Copies the player's instance of DungeonPlayerData
        DungeonGameData.Current.DungeonScene = SceneManager.GetActiveScene().name;// Store the name of the active scene
        string jsonString = JsonUtility.ToJson(DungeonGameData.Current, true);// Transforms the Data to Json format
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
    }
    private void SaveCombat()// This will save the data pertaining the combat information on a specific .Combat file
    {
        if (File.Exists(Application.persistentDataPath + "/InitialState.Default"))// If there is an initial state
            File.Delete(Application.persistentDataPath + "/InitialState.Default");// Delete it
        dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Combat");// Saves the information at this location
        Debug.Log(dataPath);
        CombatGameData.Current.CardsinHandID.Clear();// Reset the condition to store only what is currently in hand
        CombatGameData.Current.CardsinDeckID.Clear();// Reset the condition to store only what is currently in deck
        CombatGameData.Current.PlayerData = CombatPlayer.myData;
        foreach (CardInfo Card in Deck.cardsList)// Go through all cards in hand
            if (Card != null)// Check if card is not null
                CombatGameData.Current.CardsinDeckID.Add(Card.ID); // Acquire that card's ID and store it in the save file
        foreach (CardInfo Card in Hand.cardsList)// Go through all cards in hand
            if (Card != null)// Check if card is not null
                CombatGameData.Current.CardsinHandID.Add(Card.ID); // Acquire that card's ID and store it in the save file
        CombatGameData.Current.EnemyData = EnemyManager.EnemyData;// Copies this list
        CombatGameData.Current.TurnCount = TurnMaster.TurnCount;// Stores the current turn count
        CombatGameData.Current.whichCombatState = TurnManager.State;// Stores the current turn state
        CombatGameData.Current.CombatScene = SceneManager.GetActiveScene().name;// Store the name of the active scene
        string jsonString = JsonUtility.ToJson(CombatGameData.Current, true);// Transforms the Data to Json format
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
    }
    public void SaveGame()// This will save all the information on json files
    {
        if (CombatScene)// If the player is currently at a combat scene
            SaveCombat();
        else if (DungeonScene)// If the player is currently at a dungeon scene
            SaveDungeon();
    }
    public void Load()// Method called to load the player's save files and call an event to handle with that information
    {
        LoadCombat();// Load the Combat Save file
        LoadDungeon();// Load the Dungeon save file
        LoadEvent?.Invoke();// Calls all the methods subscribed to this event
        SaveGame();// Save the game after doing all the load event methods
    }
    private void LoadCombat()// Load the Combat save file
    {
        var JSONString = string.Empty;// Initializes the variable to be an empty string
        if (File.Exists(Application.persistentDataPath + "/" + PlayerPrefs.GetString("Name") +".Combat"))// If there is a save
           JSONString = File.ReadAllText(Application.persistentDataPath + "/" + PlayerPrefs.GetString("Name") +".Combat");
        else if(File.Exists(Application.persistentDataPath + "/InitialState.Default"))// If there is no save
        {
            JSONString = File.ReadAllText(Application.persistentDataPath + "/InitialState.Default");
        }
        else
        {
            StreamReader TextFile = new StreamReader("Assets/Resources/Text/CombatSkeleton.txt");// Load a skeleton combat with no info
            JSONString = TextFile.ReadToEnd();
        }
        CombatGameData.Current = JsonUtility.FromJson<CombatGameData>(JSONString);
        

    }
    private void LoadDungeon()
    {
        var JSONString = string.Empty;// Initializes the variable to be an empty string

        if (File.Exists(Application.persistentDataPath + "/" + PlayerPrefs.GetString("Name") +".Dungeon"))// If there is a save
            JSONString = File.ReadAllText(Application.persistentDataPath + "/" + PlayerPrefs.GetString("Name") +".Dungeon");
        else
        {
            StreamReader TextFile = new StreamReader("Assets/Resources/Text/DefaultSave.txt");  
            JSONString = TextFile.ReadToEnd();
        }
        DungeonGameData.Current = JsonUtility.FromJson<DungeonGameData>(JSONString);
    }
}
