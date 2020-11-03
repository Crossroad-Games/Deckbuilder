using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

public class Miasma : EnemyAction
{
    public int newAmountofDecayStacks, newDisruptDuration, newAmountofBurnStacks;
    private int AmountofDecayStacks=5, AmountofBurnStacks=10;// Apply 5 stacks of Decay and 10 Stacks of burn
    private int DisruptDuration = 3;// How long should the Disrupt effect linger
    public bool CustomDecayStacks, CustomDisruptDuration, CustomBurnStacks;
    private void Awake()
    {
        if (Customizable)// IF there are customized items
        {
            if (CustomDecayStacks)// If this property is customized
                AmountofDecayStacks = newAmountofDecayStacks;// Update the new amount of decay stacks
            if (CustomDisruptDuration)// If the disrupt has a custom duration
                DisruptDuration = newDisruptDuration;// Update the new disrupt duration
            if (CustomBurnStacks)// If this property is customized
                AmountofBurnStacks = newAmountofBurnStacks;// Update the new amount of burn stacks
        }
    }
    #region Apply Effects
    private void ApplyDecay()
    {
        DecayEffect preExistantEffect = Player.GetComponent<DecayEffect>();// Get the player's Decay Effect
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            DecayEffect effectToAdd = Player.gameObject.AddComponent<DecayEffect>() as DecayEffect;// Apply a decay effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, AmountofDecayStacks);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(AmountofDecayStacks);// Add more stacks
    }
    private void ApplyDisrupt()
    {
        DisruptedEffect preExistantEffect = Player.GetComponent<DisruptedEffect>();// Get the player's Decay Effect
        if (preExistantEffect != null)// If there is a disrupt effect
            Destroy(preExistantEffect);// Remove it
        DisruptedEffect DisruptToAdd = Player.gameObject.AddComponent<DisruptedEffect>() as DisruptedEffect;// Apply Disrupted status to the player, messing with their CD pile
        DisruptToAdd.InitializeEffect(0, 0, 0, 1, 1, DisruptDuration);// Duration of the effect
    }
    private void ApplyBurn()
    {
        BurnEffect preExistantEffect = Player.GetComponent<BurnEffect>();// Get the player's Decay Effect
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            BurnEffect effectToAdd = Player.gameObject.AddComponent<BurnEffect>() as BurnEffect;// Apply a decay effect
            effectToAdd.InitializeEffect(0, 0, 0, 1, 1, AmountofBurnStacks);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.turnCounter += AmountofBurnStacks;// Add more stacks
    }

    public override IEnumerator Effect()
    {
        ApplyDecay();// Apply decay to the player
        ApplyDisrupt();// Apply Disrupt to the player
        ApplyBurn();// Appluy burn to the player
        while (!ActionDone)
        {
            yield return new WaitForSeconds(1f);
            ActionDone = true;
            Debug.LogWarning("Needs to update this part to get ActionDone from animator and change the delay");
        }
    }
    #endregion
}
