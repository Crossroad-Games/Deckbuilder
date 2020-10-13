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
    private EnemyManager EnemyManager;
    public static Action LoadEvent;
    private void Awake()
    {
       
    }
    private void Start()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "PlaceholderFileName");// Saves the information at this location
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();// Reference to the player is defined, Save function will pull the information from this script to save it on a json file
        EnemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();// Reference to the enemy manager is defined, Save function will pull the enemy data stored
        Load();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveGame()// This will save all the information on this script to the file
    {
        GameData.Current.PlayerData = Player.myData;
        GameData.Current.EnemyData = EnemyManager.EnemyData;// Copies this array
        GameData.Current.TurnCount = TurnManager.TurnCount;// Stores the current turn count
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
