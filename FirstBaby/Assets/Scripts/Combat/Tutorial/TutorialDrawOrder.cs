using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDrawOrder : MonoBehaviour
{
    [SerializeField] public List<int> CardsIDToDraw;// Cards to be drawn in the tutorial
    [SerializeField] public List<int> DrawAmount;// How many cards are supposed to be drawn
}
