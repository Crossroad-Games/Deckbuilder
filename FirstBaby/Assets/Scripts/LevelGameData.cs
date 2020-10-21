using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelGameData
{
    public static LevelGameData Current;
    public List<bool> InterectablesUsed; //List that represent which interactables in scene were used already
    public LevelGameData()
    {
        InterectablesUsed = new List<bool>();
    }
}
