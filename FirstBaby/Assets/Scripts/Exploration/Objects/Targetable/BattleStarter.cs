using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class BattleStarter : Targetable
{
    [SerializeField] private string CombatSceneName=string.Empty;
    [SerializeField] private CombatPlayerData CombatData=null;
    [SerializeField] private List<GameObject> Enemies= new List<GameObject>();
    private DungeonPlayer Player;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<DungeonPlayer>();// Reference to the player is set
    }
    public override void ExecuteAction() => StartCoroutine(LoadScene());// Starts the coroutine that will async load the combat scene
    IEnumerator LoadScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        var iterator = 0;
        if (File.Exists(Application.persistentDataPath + "/" + PlayerPrefs.GetString("Name") + ".Combat"))// If there is a save
            File.Delete(Application.persistentDataPath + "/" + PlayerPrefs.GetString("Name") + ".Combat");// Delete it
        CombatGameData.Current.PlayerData = CombatData;// Copies the value on the inspector to this 
        CombatGameData.Current.PlayerData.PlayerLifeForce = Player.myData.PlayerLifeForce;// Updates the Combat Life Force
        CombatGameData.Current.PlayerData.Name = Player.myData.Name;// Updates the Player name
        CombatGameData.Current.CardsinHandID.Clear();// Cleans the list of cards in hand
        CombatGameData.Current.CardsinDeckID = Player.myData.CardCollectionID;// Copies the player's card collection
        CombatGameData.Current.EnemyData.Clear();// Cleans the list of enemy data
        foreach (GameObject Enemy in Enemies)// Go through the list of enemies in this encounter
            if (Enemies != null)// If enemy not null
            {
                var EnemyInfo = Enemy.GetComponent<EnemyClass>().myData;
                var Data = new EnemyData(EnemyInfo.ID, EnemyInfo.EnemyName, EnemyInfo.EnemyHP, EnemyInfo.EnemyMaxHP, EnemyInfo.EnemyDefense, EnemyInfo.EnemyShield, iterator);
                CombatGameData.Current.EnemyData.Add(Data);// Acquires each enemy data and store it
                iterator++;
            }
        CombatGameData.Current.TurnCount = 0;// Start on turn 0
        CombatGameData.Current.whichCombatState = 0;// Start at the player's turn start
        string jsonString = JsonUtility.ToJson(CombatGameData.Current, true);// Transforms the Data to Json format
        using (StreamWriter streamWriter = File.CreateText(Application.persistentDataPath + "/InitialState.Default"))// Creates a text file with that path
        {
            streamWriter.Write(jsonString);// Writes the content in json format
        }
        GameObject.Find("Game Master").GetComponent<SaveLoad>().SaveGame();// Save the dungeon data
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(CombatSceneName, LoadSceneMode.Single);// Loads the combat scene
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
