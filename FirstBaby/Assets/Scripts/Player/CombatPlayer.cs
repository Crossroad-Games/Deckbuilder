using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
    [SerializeField] private CardPile deck;
    [SerializeField] private CardPile hand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            deck.SendCard(deck.cardsList[0], hand); // This would be the draw, sending the first card of deck to hand
            Debug.Log("Chamou SendCard");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            deck.Shuffle(); //shuffles the Deck
        }
    }
}
