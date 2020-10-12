using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("Player Information")]
    #region Player Information
    [SerializeField] public int PlayerHP; // Current HP
    [SerializeField] public int PlayerMaxHP;// Maximum HP
    [SerializeField] public int PlayerDefense;// Player Defense stat
    [SerializeField] public string Name;// Could be either a username or a preset name?
    #endregion
}
