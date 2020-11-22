using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelGameData
{
    public static LevelGameData Current;
    public List<bool> InterectablesUsed; //List that represent which interactables in scene were used already
    public ListWrapper ObjectsLayer;// List of each object that may be controlled by a door current layer
    public int whichDoor;// Which door is controlling the game layer state
    public LevelGameData()
    {
        InterectablesUsed = new List<bool>();
    }
}
[System.Serializable]
public class ListWrapper
{
    public List<int> LayerList;
}