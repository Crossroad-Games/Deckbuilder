using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static GameData Current;
    public PlayerData PlayerData;
    public List<int> CardsinHandID;
    public List<int> CardsinDeckID;
    public EnemyData[] EnemyData;
    public int TurnCount;
    public CombatState whichCombatState;
    public GameData()
    {
        PlayerData = new PlayerData();
        CardsinHandID = new List<int>();
        CardsinDeckID = new List<int>();
        EnemyData = new EnemyData[5];
        TurnCount = 0;
        whichCombatState = new CombatState();
    }
}
