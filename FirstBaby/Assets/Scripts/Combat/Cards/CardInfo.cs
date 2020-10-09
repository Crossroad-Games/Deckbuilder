using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newCardInfo", menuName = "Data/Card/Card Info")]
public class CardInfo : ScriptableObject
{
    public string name;// name of the card
    public int ID;// ID of the card
    public string Description;
    public Sprite sprite;
    public GameObject cardPrefab;
}
