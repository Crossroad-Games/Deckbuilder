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
