using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private string dataPath;// Where it will be saved to in PC
    private CombatPlayer CombatPlayer;// Reference to the Combat Player will be used to get the player's combat information
    private DungeonPlayer DungeonPlayer;// Reference to the Dungeon Player will be used to get the player's dungeon information
    private Hand Hand;// Will be used to access the list of all the cards in your hand
    private Deck Deck;// Will be used to access the list of all the cards in your deck
    private CDPile CDPile;// Will be used to access the list of all the cards in your CD pile and their current CDs
    public bool CombatScene = false, DungeonScene = false;// Booleans used to check if the player is either on a combat or dungeon scene 
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
            CDPile = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CDPile>();// Reference to the CD Pile is defined, save function will pull the information from it to store the cards that are currently at the CD Pile and their CDs on the save file
            EnemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();// Reference to the enemy manager is defined, Save function will pull the enemy data stored
            TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();// Reference to the turn manager is defined, save function will pull the turn count and combat state to store it on the save file
        }
        else if (DungeonScene)// If the player is on Dungeon Scene
        {
            DungeonPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();// Reference to the player is defined
        }
        Load();// Load the save files
    }
    private void SaveDungeon()// Method called to save the player's dungeon information
    {
        #region Checkpoint save
        string jsonString = JsonUtility.ToJson(DungeonGameData.Current, true);// Transforms the Data to Json format
        dataPath= Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Previous");// Path to a copy of your current state to be stored as a previous save points, accessed in case of player death
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
        #endregion
        #region Current Dungeon Player save
        dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Dungeon");// Saves the information at this location
        DungeonGameData.Current.PlayerPosition = DungeonPlayer.transform.position;// Stores the player's position 
        DungeonGameData.Current.PlayerData = DungeonPlayer.myData;// Copies the player's instance of DungeonPlayerData
        DungeonGameData.Current.DungeonScene = SceneManager.GetActiveScene().name;// Store the name of the active scene
        List<Interactable> dungeonInteractables = GameObject.Find("Game Master").GetComponent<InteractableDatabase>().InsteractablesInScene;
        DungeonGameData.Current.InterectablesUsed.Clear();// Clear the list before adding all the information
        foreach (Interactable inter in dungeonInteractables)
            DungeonGameData.Current.InterectablesUsed.Add(inter.Used); //Go through all interactables in current scene and change the data file to contain the used infos
        jsonString = JsonUtility.ToJson(DungeonGameData.Current, true);// Transforms the Data to Json format
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
        #endregion
        #region Current Scene Objects save
        dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") +"."+ DungeonGameData.Current.DungeonScene);// Type of file for each dungeon scene
        Debug.Log("Save file path: " + dataPath);
        LevelGameData.Current.InterectablesUsed.Clear();// Clear the list before adding all the information
        foreach (Interactable inter in dungeonInteractables)
            LevelGameData.Current.InterectablesUsed.Add(inter.Used); //Go through all interactables in current scene and change the data file to contain the used infos
        jsonString = JsonUtility.ToJson(LevelGameData.Current, true);// Transforms the Data to Json format
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
        #endregion
    }
    private void SaveCombat()// This will save the data pertaining the combat information on a specific .Combat file
    {
        if (File.Exists(Application.persistentDataPath + "/InitialState.Default"))// If there is an initial state
            File.Delete(Application.persistentDataPath + "/InitialState.Default");// Delete it
        dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Combat");// Saves the information at this location
        CombatGameData.Current.CardsinHandID.Clear();// Reset the condition to store only what is currently in hand
        CombatGameData.Current.CardsinDeckID.Clear();// Reset the condition to store only what is currently in deck
        CombatGameData.Current.CardsinCD.Clear();// Reset the condition to store only what is currently in the CD Pile
        CombatGameData.Current.CardsCD.Clear();// Reset the condition to store only the CD values of the cards currently on CD
        CombatGameData.Current.PlayerData = CombatPlayer.myData;
        foreach (GameObject Card in Deck.cardsList)// Go through all cards in hand
            if (Card != null)// Check if card is not null
                CombatGameData.Current.CardsinDeckID.Add(Card.GetComponent<VirtualCard>().cardInfo.ID); // Acquire that card's ID and store it in the save file
        foreach (GameObject Card in Hand.cardsList)// Go through all cards in hand
            if (Card != null)// Check if card is not null
                CombatGameData.Current.CardsinHandID.Add(Card.GetComponent<VirtualCard>().cardInfo.ID); // Acquire that card's ID and store it in the save file
        foreach(GameObject Card in CDPile.cardsList)// Go through all the cards in the CD Pile
            if(Card!=null)// Check if card is not null
            {
                CombatGameData.Current.CardsinCD.Add(Card.GetComponent<VirtualCard>().cardInfo.ID);// Acquire that card's ID and store it in the save file
                CombatGameData.Current.CardsCD.Add(Card.GetComponent<VirtualCard>().CurrentCooldownTime);// Acquire that card's CD and store it
            }
        CombatGameData.Current.EnemyData = EnemyManager.EnemyData;// Copies this list
        /*CombatGameData.Current.TurnCount = TurnMaster.TurnCount;// Stores the current turn count
        CombatGameData.Current.whichCombatState = TurnManager.State;// Stores the current turn state*/
        CombatGameData.Current.CombatScene = SceneManager.GetActiveScene().name;// Store the name of the active scene
        string jsonString = JsonUtility.ToJson(CombatGameData.Current, true);// Transforms the Data to Json format
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
    }
    public void SaveGame()// This will save all the information on json files
    {
        Debug.Log("Saved");
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
        dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Combat");
        if (File.Exists(dataPath))// If there is a save
            JSONString = File.ReadAllText(dataPath);
        else if (File.Exists(Application.persistentDataPath + "/InitialState.Default"))// If there is no save
        {
            Debug.Log("Loaded Initial State, Deck ID List:");
            JSONString = File.ReadAllText(Application.persistentDataPath + "/InitialState.Default");
        }
        CombatGameData.Current = JsonUtility.FromJson<CombatGameData>(JSONString);

    }
    private void LoadDungeon()
    {
        var JSONString = string.Empty;// Initializes the variable to be an empty string
        dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Dungeon");
        if (File.Exists(dataPath))// If there is a save
            JSONString = File.ReadAllText(dataPath);
        else
        {
            TextAsset myFile = null;
            switch(PlayerPrefs.GetString("Name"))
            {
                case "Alchemist":
                    myFile = Resources.Load<TextAsset>("Text/AlchemistSave");
                    break;
                case "Elementalist":
                    myFile = Resources.Load<TextAsset>("Text/ElementalistSave");
                    break;
                default:
                    myFile = Resources.Load<TextAsset>("Text/DefaultSave");
                    break;
            }
            JSONString = myFile.text;
        }
        DungeonGameData.Current = JsonUtility.FromJson<DungeonGameData>(JSONString);
        while (DungeonGameData.Current.PlayerData.CardLevels.Count < GetComponent<CardDatabase>().GameCards.Count)// If the save has less ID's than the Database
            DungeonGameData.Current.PlayerData.CardLevels.Add(0);// Add a 0 LVL version of that
        Debug.Log(DungeonGameData.Current.PlayerData.CardLevels.Count);
        dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + "." + SceneManager.GetActiveScene().name);// Locate that dungeon's save
        if (File.Exists(dataPath))// If there is a dungeon scene attached to this save
                JSONString = File.ReadAllText(dataPath);// Read the dungeon scene objects save
        else
        {
            TextAsset myFile = Resources.Load<TextAsset>("Text/DefaultDungeon");
            JSONString = myFile.text;
        }
        LevelGameData.Current = JsonUtility.FromJson<LevelGameData>(JSONString);// Load it into this object
        List<Interactable> dungeonInteractables = GameObject.Find("Game Master").GetComponent<InteractableDatabase>().InsteractablesInScene;// Reference to the list of interactables on that scene
        if (dungeonInteractables.Count == LevelGameData.Current.InterectablesUsed.Count)// Verify if information matches
        {
            for (int i = 0; i < dungeonInteractables.Count; i++)// Updates all values
            {
                dungeonInteractables[i].Used = LevelGameData.Current.InterectablesUsed[i]; // Go through all interactables and scene and convert get from data file the used infos
            }
        }
    }
}
