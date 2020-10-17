using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public DungeonPlayerData myData;// Player's Dungeon information such as scene, name and resources
    private void Awake()
    {
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
        myData.Name = PlayerPrefs.GetString("Name");// Set the player's name to be the one on the player pref
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
