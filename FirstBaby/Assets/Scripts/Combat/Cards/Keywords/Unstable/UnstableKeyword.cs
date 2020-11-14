using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableKeyword : CardExtension
{
    protected Deck mydeck;
    protected CDPile myCDPile;
    [SerializeField]public int UnstableIntensity=1;// Mill X cards
    protected override void Awake()
    {
        base.Awake();
        Keyword = "Unstable";
        mydeck = combatPlayer.GetComponent<Deck>();// Reference to the deck is set
        myCDPile = combatPlayer.gameObject.GetComponentInChildren<CDPile>();// Reference to the CD Pile is defined
    }
    public override void ExtensionEffect()
    {
        for (var iterator = UnstableIntensity;iterator>0;iterator--)// Do X times
            if(mydeck.cardsList.Count>0)// If deck is not empty
                mydeck.SendCard(mydeck.cardsList[0], myCDPile);// Mill the first card
    }
}
