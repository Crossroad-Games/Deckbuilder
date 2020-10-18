using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public DungeonPlayerData myData;// Player's Dungeon information such as scene, name and resources
    private SaveLoad Saver;// Saveload reference will be used to save the game when the player starts a dungeon scene
    private void Awake()
    {
        Saver = GameObject.Find("Game Master").GetComponent<SaveLoad>();// Reference to the saveload script is defined
        SaveLoad.LoadEvent += LoadData;// Subscribe this method to the event
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LoadData()
    {
        transform.position = DungeonGameData.Current.PlayerPosition;// Syncs the player's position to the one on the save file
        myData = DungeonGameData.Current.PlayerData;// Syncs the player's information to the one on the save file
        myData.Name = PlayerPrefs.GetString("Name");
        Saver.SaveGame();// Save the game when entering a dungeon scene
    }
    private void OnDisable()
    {
        SaveLoad.LoadEvent -= LoadData;// Subscribe this method to the event
    }
    public void NearInteractable()
    {

    }
    public void Interacting()
    {
        Debug.Log("Interacted");
    }
}
