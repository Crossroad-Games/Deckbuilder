using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private string dataPath;// Where it will be saved to in PC
    private CombatPlayer Player;
    private Hand Hand;// Will be used to access the list of all the cards in your hand
    private Deck Deck;// Will be used to access the list of all the cards in your deck
    [SerializeField]private bool CombatScene=false, DungeonScene=false;// Booleans used to check if the player is either on a combat or dungeon scene 
    private EnemyManager EnemyManager;
    private TurnManager TurnMaster;
    public static Action LoadEvent;
    private void Awake()
    {
       
    }
    private void Start()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "PlaceholderFileName.Combat");// Saves the information at this location
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();// Reference to the player is defined, Save function will pull the information from this script to save it on a json file
        Hand= GameObject.FindGameObjectWithTag("Player").GetComponent<Hand>();// Reference to the Hand script is defined, save function will pull the information from it to store the cards that are currently at hand
        Deck = GameObject.FindGameObjectWithTag("Player").GetComponent<Deck>();// Reference to the Hand script is defined, save function will pull the information from it to store the cards that are currently at deck
        EnemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();// Reference to the enemy manager is defined, Save function will pull the enemy data stored
        TurnMaster = GameObject.Find("Turn Master").GetComponent<TurnManager>();// Reference to the turn manager is defined, save function will pull the turn count and combat state to store it on the save file
        Load();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void SaveDungeon()
    {

    }
    private void SaveCombat()// This will save the data pertaining the combat information on a specific .Combat file
    {
        CombatGameData.Current.CardsinHandID.Clear();// Reset the condition to store only what is currently in hand
        CombatGameData.Current.CardsinDeckID.Clear();// Reset the condition to store only what is currently in deck
        CombatGameData.Current.PlayerData = Player.myData;
        foreach (CardInfo Card in Deck.cardsList)// Go through all cards in hand
            if (Card != null)// Check if card is not null
                CombatGameData.Current.CardsinDeckID.Add(Card.ID); // Acquire that card's ID and store it in the save file
        foreach (CardInfo Card in Hand.cardsList)// Go through all cards in hand
            if (Card != null)// Check if card is not null
                CombatGameData.Current.CardsinHandID.Add(Card.ID); // Acquire that card's ID and store it in the save file
        CombatGameData.Current.EnemyData = EnemyManager.EnemyData;// Copies this list
        CombatGameData.Current.TurnCount = TurnMaster.TurnCount;// Stores the current turn count
        CombatGameData.Current.whichCombatState = TurnManager.State;// Stores the current turn state
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
        if (DungeonScene)// If the player is currently at a dungeon scene
            SaveDungeon();
    }
    public void Load()
    {
        LoadCombat();
        LoadDungeon();
    }
    private void LoadCombat()
    {
        var JSONString = string.Empty;// Initializes the variable to be an empty string
        if (File.Exists(Application.persistentDataPath + "/PlaceholderFileName.Combat"))// If there is a save
           JSONString = File.ReadAllText(Application.persistentDataPath + "/PlaceholderFileName.Combat");
        else if(File.Exists(Application.persistentDataPath + "/DefaultSave.Combat"))// If there is no save
            JSONString = File.ReadAllText(Application.persistentDataPath + "/DefaultSave.Combat");
        CombatGameData.Current = JsonUtility.FromJson<CombatGameData>(JSONString);
        LoadEvent?.Invoke();// Calls all the methods subscribed to this event
    }
    private void LoadDungeon()
    {

    }
}
