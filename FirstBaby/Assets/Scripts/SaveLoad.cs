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
    private EnemyManager EnemyManager;
    private TurnManager TurnMaster;
    public static Action LoadEvent;
    private void Awake()
    {
       
    }
    private void Start()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "PlaceholderFileName");// Saves the information at this location
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
    public void SaveGame()// This will save all the information on this script to the file
    {
        GameData.Current.CardsinHandID.Clear();// Reset the condition to store only what is currently in hand
        GameData.Current.CardsinDeckID.Clear();// Reset the condition to store only what is currently in deck
        GameData.Current.PlayerData = Player.myData;
        foreach (CardInfo Card in Deck.cardsList)// Go through all cards in hand
            if (Card != null)// Check if card is not null
                GameData.Current.CardsinDeckID.Add(Card.ID); // Acquire that card's ID and store it in the save file
        foreach (CardInfo Card in Hand.cardsList)// Go through all cards in hand
            if (Card != null)// Check if card is not null
                GameData.Current.CardsinHandID.Add(Card.ID); // Acquire that card's ID and store it in the save file
        GameData.Current.EnemyData = EnemyManager.EnemyData;// Copies this list
        GameData.Current.TurnCount = TurnMaster.TurnCount;// Stores the current turn count
        GameData.Current.whichCombatState = TurnManager.State;// Stores the current turn state
        string jsonString = JsonUtility.ToJson(GameData.Current,true);// Transforms the Data to Json format
        Debug.Log(Application.persistentDataPath);
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
    }
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/PlaceholderFileName"))
        {
            //FileStream file = File.Open(Application.persistentDataPath + "/PlaceholderFileName", FileMode.Open);
            string JSONString = File.ReadAllText(Application.persistentDataPath + "/PlaceholderFileName");
            //Current = JsonUtility.FromJson<GameData>(file.ToString());
            GameData.Current = JsonUtility.FromJson<GameData>(JSONString);
            // file.Close();
            LoadEvent?.Invoke();// Calls all the methods subscribed to this event
        }
    }
}
