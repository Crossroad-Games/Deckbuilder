using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMirror : Interactable
{
    public override void Actuated()
    {
        Player.GetComponent<PlayerMovement>().canMove = false; //Disable player movement
    }

    public override void LoadDungeonState()
    {
        
    }
}
