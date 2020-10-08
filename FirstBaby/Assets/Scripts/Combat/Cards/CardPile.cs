using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPile : MonoBehaviour
{
    public List<CardInfo> cardsList;

    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void SendCard(CardInfo cardToSend, CardPile target)
    {
        cardsList.Remove(cardToSend);
        target.ReceiveCard(cardToSend);
    }

    public void ReceiveCard(CardInfo cardToReceive)
    {
        cardsList.Add(cardToReceive);
    }
}
