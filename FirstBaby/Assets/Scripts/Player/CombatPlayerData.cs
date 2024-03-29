﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatPlayerData
{
    [Header("Player Information")]
    #region Player Information
    [SerializeField] public int PlayerLifeForce; // Current HP
    [SerializeField] public int PlayerDefense;// Player Defense stat
    [SerializeField] public int PlayerShield;// Player Shield stat
    [SerializeField] public string Name;// Could be either a username or a preset name?
    #endregion
}
