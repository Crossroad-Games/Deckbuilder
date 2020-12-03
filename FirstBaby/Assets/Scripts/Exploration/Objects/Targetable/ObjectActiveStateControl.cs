using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActiveStateControl : Targetable
{
    [SerializeField] private List<GameObject> GOtoControl= new List<GameObject>();// List of objects this targetable will be controlling
    public override void ExecuteAction()
    {
        foreach (GameObject GO in GOtoControl)// Cycle through each GO in the list
            if (GO != null)// IF not null
                GO.SetActive(!GO.activeSelf);// Flip its current state
    }
}
