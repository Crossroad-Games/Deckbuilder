using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptedEffect : LastingEffect
{
    protected override void Awake()
    {
        Debug.Log("Added the effect");
        player = GetComponent<CombatPlayer>();
        player.Disrupted = true;// Disrupts the player until this effect duration ends
        TurnManager.PlayerTurnStart += Effect;
    }
    protected override void OnDisable()
    {
        Debug.Log("Destroyed the effect");
        player.Disrupted = false;// No longer disrupting the player
        TurnManager.PlayerTurnStart -= Effect;
    }
}
