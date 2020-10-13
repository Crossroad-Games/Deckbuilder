using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : CardPile
{
    #region References
    [SerializeField] private CardPile hand;
    #endregion


    void Start()
    {
        // Initialize Deck randomly for testing porpuses
        for(int i=0; i < 20; i++)
        {
            int n = Random.Range(0, 3);
            CardInfo cardInfoInstance = Object.Instantiate(cardDatabase.GameCards[n]);
            cardsList.Add(cardInfoInstance);
        }
    }

    void Update()
    {
        
    }
}
