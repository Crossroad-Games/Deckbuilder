using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newCardInfo", menuName = "Data/Card/Card Info")]
public class CardInfo : ScriptableObject
{
    public enum CardRarity { Common, Uncommon, Rare, UltraRare }
    public CardRarity rarity;
    public string Name;// name of the card
    public int ID;// ID of the card
    public string Description;
    public CardPorpuse cardPorpuse;
    public int Cooldown;
    public int ResourceCost = 10;
    public Sprite sprite;
    public GameObject cardPrefab;
}
