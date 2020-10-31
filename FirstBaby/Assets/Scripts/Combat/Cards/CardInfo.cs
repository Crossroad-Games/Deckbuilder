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
    public bool Attack = false;
    public bool Defense = false;
    public bool SpecialCard = false;
    public int Cooldown;
    public int ResourceCost = 10;
    [SerializeField]private int currentCooldownTime;
    public int CurrentCooldownTime
    {
        get { return currentCooldownTime; }
        set { currentCooldownTime = value; }
    }
    public Sprite sprite;
    public GameObject cardPrefab;
    [SerializeField] private Card myPhysicalCard; //Card this cardInfo is  attached to when in hand
    public Card MyPhysicalCard
    {
        get
        {
            if (myPhysicalCard == null)
            {
                throw new MissingReferenceException("This CardInfo doesn't have a Card linked to");
            }
            return myPhysicalCard;
        }
        set
        {
            myPhysicalCard = value;
        }
    }
}
