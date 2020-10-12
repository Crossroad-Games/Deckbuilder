﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static GameData Current;
    public PlayerData PlayerData;
    public EnemyData[] EnemyData;
    public GameData()
    {
        PlayerData = new PlayerData();
        EnemyData = new EnemyData[4];
    }
}
