using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public override void Actuated()
    {
        Debug.Log("Get some rewards");
    }
}
