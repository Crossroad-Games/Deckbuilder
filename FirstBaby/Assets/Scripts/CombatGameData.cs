using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatGameData
{
    public static CombatGameData Current;
    public CombatPlayerData PlayerData;
    public List<int> CardsinHandID;
    public List<int> CardsinDeckID;
    public List<EnemyData> EnemyData;
    public int TurnCount;
    public CombatState whichCombatState;
    public string CombatScene;
    public CombatGameData()
    {
        PlayerData = new CombatPlayerData();
        CardsinHandID = new List<int>();
        CardsinDeckID = new List<int>();
        EnemyData = new List<EnemyData>();
        TurnCount = 0;
        whichCombatState = new CombatState();
        CombatScene = string.Empty;
    }
}
