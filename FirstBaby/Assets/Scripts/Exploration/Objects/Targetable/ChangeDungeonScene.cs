using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ChangeDungeonScene : Targetable
{
    [SerializeField] private string DungeonSceneName= string.Empty;
    [SerializeField] private Vector3 SpawnPosition= new Vector3();
    public override void ExecuteAction()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
       
        this.GetComponent<Door>().Used = false;// Set the used boolean to false so the player can keep going back and forth between scenes
        GameObject.Find("Game Master").GetComponent<SaveLoad>().SaveGame();// Save the current state before changing scene
        DungeonGameData.Current.PlayerPosition = SpawnPosition;// Player will spawn at this position
        var jsonString = JsonUtility.ToJson(DungeonGameData.Current, true);// Transforms the Data to Json format
        var dataPath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Name") + ".Dungeon");// Saves the information at this location
        using (StreamWriter streamWriter = File.CreateText(dataPath))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(DungeonSceneName, LoadSceneMode.Single);// Loads the combat scene
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
    }
}
