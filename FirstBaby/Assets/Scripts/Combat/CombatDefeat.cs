using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class CombatDefeat : MonoBehaviour
{
    #region References
    private Button EndTurnButton = null;
    private GameObject DefeatScreen = null;
    private CombatPlayer Player = null;
    private CombatManager combatManager = null;
    #endregion

    #region Events
    public static Action playerDefeatEvent;
    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        DefeatScreen = GameObject.Find("Defeat Screen");
        DefeatScreen.SetActive(false);
    }
    void Start()
    {
        EndTurnButton = GameObject.Find("Canvas").transform.Find("End Turn").GetComponent<Button>();// Reference to the end button is set
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();// Reference to the player is set
        combatManager = GameObject.Find("Combat Manager").GetComponent<CombatManager>();// Reference to the combat manager
       
    }


    public void Defeat()
    {
        playerDefeatEvent?.Invoke();// Calls the event for when player loses a combat
        EndTurnButton.gameObject.SetActive(false);// Deactivates the button
        Player.gameObject.GetComponent<Hand>().DiscardHand();// Discard the cards in hand
        DefeatScreen.SetActive(true);
        combatManager.Defeated = true;

    }

    public void Retry()
    {
        var dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Previous");// Path to the previous save
        var JSONString = string.Empty;// Empty string will be used to read the text on the file and store the game data
        if (File.Exists(dataPath))// If there is a save
            JSONString = File.ReadAllText(dataPath);// Store the save file on a string
        DungeonGameData.Current = JsonUtility.FromJson<DungeonGameData>(JSONString);// Converts the JSON string to a Dungeon Game Data
        JSONString = JsonUtility.ToJson(DungeonGameData.Current, true);// Transforms the Data to Json format
        dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Dungeon");// Path to the previous save
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(JSONString);// Writes the content in json format
        }
        dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Combat");// Acquires the path to the combat file
        if (File.Exists(dataPath))// If there is an initial state
            File.Delete(dataPath);// Delete it
        SceneManager.LoadSceneAsync(DungeonGameData.Current.DungeonScene, LoadSceneMode.Single);// Loads the Dungeon Scene
    }
}
