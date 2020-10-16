using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonGameData
{
    public static DungeonGameData Current;
    public Vector3 PlayerPosition;// Player vector 3 position on the dungeon scene
    public DungeonPlayerData PlayerData; // Player information
    public string DungeonScene; // Which dungeon scene the player is currently at
    public DungeonGameData()
    {
        PlayerPosition = new Vector3(0, 0, 0);
        PlayerData = new DungeonPlayerData();
        DungeonScene = string.Empty;
    }
}
