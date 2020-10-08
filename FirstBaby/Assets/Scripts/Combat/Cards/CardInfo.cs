using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardInfo : ScriptableObject
{
    [SerializeField] public string name { get; private set; } // name of the card
    [SerializeField] public int ID { get; private set; }  // ID of the card
    [SerializeField] public string Description { get; private set; }

    
}
