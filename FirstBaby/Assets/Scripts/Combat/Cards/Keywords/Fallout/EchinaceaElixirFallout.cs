using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchinaceaElixirFallout : FalloutKeyword
{
    [SerializeField] private int AmountofStacks = 5;

    public override void FalloutEffect()
    {
        combatPlayer.GainShield(8);
        WallEfect preExistantEffect = combatPlayer.GetComponent<WallEfect>();// Get the player's Decay Effect
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            WallEfect effectToAdd = combatPlayer.gameObject.AddComponent<WallEfect>() as WallEfect;// Apply a decay effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, AmountofStacks);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(AmountofStacks);// Add more stacks
    }
}
