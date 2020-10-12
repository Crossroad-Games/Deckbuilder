using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public int ID;// Unique identifier of this Enemy
    public string EnemyName;
    public int EnemyHP;// Current HP
    public int EnemyMaxHP;// Maximum HP
    public int EnemyDefense;// Constant removed from incoming damage
}
