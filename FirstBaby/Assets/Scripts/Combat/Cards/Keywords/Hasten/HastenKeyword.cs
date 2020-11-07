using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HastenKeyword : CardExtension
{
    protected CDPile myCDPile;
    [SerializeField] protected int HastenAmount;// Number of cards that will have their CD reduced
    protected override void Awake()
    {
        base.Awake();
        Keyword = "Hasten";
        myCDPile = combatPlayer.gameObject.GetComponentInChildren<CDPile>();// Reference to the CD Pile is defined
    }
    public override void ExtensionEffect()
    {
        for(var iterator=HastenAmount; iterator>0;iterator--)// Reduce X cards CD by 1, up to 0, does not send the card back to the deck
        {
            var RandomNumber = Random.Range(0, myCDPile.cardsList.Count);
            var RandomCard = myCDPile.cardsList[RandomNumber].GetComponent<VirtualCard>();// Picks a card at random
            //Debug.Log("Random Number: "+RandomNumber+"\nCard affected: "+RandomCard+" \n Current CD: "+RandomCard.CurrentCooldownTime);
            myCDPile.UpdateCooldown(RandomCard);// Reduces its CD by one, if it is already 0 then send it to the CD Completed, wasting only 1 Hasten on it
        }
    }
}
