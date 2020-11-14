using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonPlayerData
{
    [Header("Player Information")]
    #region Player Information
    [SerializeField] public string Name; // Could be either a username or a preset name?
    [SerializeField] public int PlayerLifeForce; // Current amount of resource used to craft cards and fight battles
    [SerializeField] public List<int> CardCollectionID;// List of cards the player currently has, non shuffled
    [SerializeField] public List<int> CardLevels;// List of card lvls
    #endregion
}
