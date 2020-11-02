using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayingBreath : EnemyAction
{
    public int newAmountofStacks;// How many stacks of decay this attack will inflict as a modified value
    private int AmountofStacks=5;// How many stacks of decay this attack will inflict as a base value
    public bool CustomStacks;// Check if this action has customized stacks
    private void Awake()
    {
        if (Customizable)// If this action has customized properties
            if (CustomStacks)// If this action's stacks are customized
                AmountofStacks = newAmountofStacks;// Update the amount of stacks this action will apply
    }
    public override void Effect()
    {
        DecayEffect preExistantEffect = Player.GetComponent<DecayEffect>();// Get the player's Decay Effect
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            DecayEffect effectToAdd = Player.gameObject.AddComponent<DecayEffect>() as DecayEffect;// Apply a decay effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, AmountofStacks);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(AmountofStacks);// Add more stacks
        Player.ProcessDamage(myClass,CalculateAction(myInfo.BaseDamage));// Deal damage to the player
    }
}
