using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    private List<DungeonGameData> GameData = new List<DungeonGameData>();// List of Dungeon saves on the save folder
    private Button[] SaveButtons = new Button[3];// Has room for 3 saves
    [SerializeField]private Button[] DeleteSaveButtons = new Button[3];// One for each save
    private SaveLoad SaveLoad;// Reference to the saveload will be used to load the chosen file when the button is clicked
    private GameObject ConfirmNewSave;// Reference to the confirm new save button
    [SerializeField] private string GameScene= string.Empty;
    public void StartGame()
    {
        if (CombatGameData.Current != null)// If there is a combat save
        {
            // If there is saved scene string and the save file name is the same as your name
            if (CombatGameData.Current.CombatScene != string.Empty && CombatGameData.Current.PlayerData.Name==PlayerPrefs.GetString("Name"))
                GameScene = CombatGameData.Current.CombatScene;// Go to this scene
        }
        else
        {
            if (DungeonGameData.Current != null)// If there is a dungeon save
                if (DungeonGameData.Current.DungeonScene != string.Empty)// If there is a saved scene string
                    GameScene = DungeonGameData.Current.DungeonScene;// Go to this scene
        }
        SceneManager.LoadSceneAsync(GameScene, LoadSceneMode.Single);// Loads the Dungeon Scene

    }
    public void QuitGame()
    {
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();// Closes the application
        #endif
    }
    #region Save Manager
    private void Awake()
    {
        SaveLoad = GameObject.Find("Game Master").GetComponent<SaveLoad>();// Reference is defined
        SaveButtons = transform.Find("Save Files").GetComponentsInChildren<Button>();// Acquires all the buttons that will be used to display and choose all the save files
        DeleteSaveButtons= transform.Find("Delete Saves").GetComponentsInChildren<Button>();// Acquire all the buttons that will destroy the save files
        foreach (Button button in DeleteSaveButtons)// Go through all the delete buttons
            if (button != null)// Check if its null
                button.gameObject.SetActive(false);// Deactivate the button
        DirectoryInfo SaveFolder = new DirectoryInfo(Application.persistentDataPath);// Folder path
        var JSONString = string.Empty;// Empty string will store all the text in the save file
        var iterator = 0;// Iterator will be used to link the data information to the save button text
        foreach (FileInfo SaveFile in SaveFolder.GetFiles().Where(File => File.Name.EndsWith(".Dungeon")))// Go through all .Dungeon Files on the folder
        {
            StreamReader SR = new StreamReader(SaveFile.FullName);// Creates a reading path to that file
            JSONString = SR.ReadToEnd();// Read the whole file and store it on a string
            GameData.Add(JsonUtility.FromJson<DungeonGameData>(JSONString));// Convert the JSON string to DungeonGameData
            SaveButtons[iterator].gameObject.transform.Find("Username").GetComponent<TMP_Text>().text = GameData[iterator].PlayerData.Name;// Sets the button's text to be the username on the save file
            SaveButtons[iterator].interactable = true;// You can choose this save
            DeleteSaveButtons[iterator].gameObject.SetActive(true);// Activates this gameobject
            iterator++;// Increment iterator 
        }
        if (iterator == 3)// If there are three saves 
            transform.Find("New Save Button").gameObject.SetActive(false);// Can't create new saves
    }
    public void ChooseSave(int SaveNumber)// Called when clicked on the button, receiving the chosen save number as a parameter
    {
        PlayerPrefs.SetString("Name", GameData[SaveNumber].PlayerData.Name);// Set the Player Name based on that save's player name
        SaveLoad.Load();// Load the game with this username
    }
    public void NewGame()
    {
        transform.Find("Save Files").gameObject.SetActive(false);// Deactivate the Load Game menu
        transform.Find("New Save File").gameObject.SetActive(true);// Activates the Username Input Field
        if (ConfirmNewSave == null)// If there is not reference
            ConfirmNewSave = transform.Find("New Save File").Find("Confirm Username").gameObject;// Reference is defined
    }
    public void DeleteGame(int SaveNumber)// Deletes all save files with that name
    {
        var dataPath = Path.Combine(Application.persistentDataPath, GameData[SaveNumber].PlayerData.Name + ".Combat");// Delete the information at this location
        if (File.Exists(dataPath))// If there is an initial state
            File.Delete(dataPath);// Delete it
        dataPath = Path.Combine(Application.persistentDataPath, GameData[SaveNumber].PlayerData.Name + ".Dungeon");// Delete the information at this location
        if (File.Exists(dataPath))// If there is an initial state
            File.Delete(dataPath);// Delete it
        dataPath = Path.Combine(Application.persistentDataPath, GameData[SaveNumber].PlayerData.Name + ".Previous");// Delete the information at this location
        if (File.Exists(dataPath))// If there is an initial state
            File.Delete(dataPath);// Delete it
    }
    public void UsernameInput(string Username)
    {
        if (Username == string.Empty)// If Username field is empty
            return;// Return
        else// If there is any information on the field
        {
            foreach (DungeonGameData Save in GameData)// Go through all the saves
                if (Save != null)// Check if its null before checking its name
                    if (Save.PlayerData.Name != Username)// Verify if the user's input is not going to override any other saves
                    {
                        PlayerPrefs.SetString("Name", Username);// Set the Player Name based on the user input
                        ConfirmNewSave.GetComponent<Button>().interactable = true;// You can't confirm your name selection
                        ConfirmNewSave.GetComponentInChildren<TMP_Text>().text = "Start Save";// Display to the player that this username is already taken
                    }   
                    else
                    {
                        ConfirmNewSave.GetComponent<Button>().interactable = false;// You can't confirm your name selection
                        ConfirmNewSave.GetComponentInChildren<TMP_Text>().text = "Name taken";// Display to the player that this username is already taken
                    }
        }
           
    }
    #endregion
}
