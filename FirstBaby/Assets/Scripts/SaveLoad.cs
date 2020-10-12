using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private string dataPath;// Where it will be saved to in PC
    private CombatPlayer Player;
    private EnemyManager EnemyManager;
    public GameData Current= new GameData();
    void Start()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "PlaceholderFileName");// Saves the information at this location
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatPlayer>();
        EnemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveGame()// This will save all the information on this script to the file
    {
        Current.PlayerData = Player.myData;
        Current.EnemyData = EnemyManager.EnemyData;// Copies this array
        string jsonString = JsonUtility.ToJson(Current,true);// Transforms the Data to Json format
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
            Current = JsonUtility.FromJson<GameData>(JSONString);
           // file.Close();
        }
    }
}
