using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockArmorCard : NonTargetCard
{
    [SerializeField] private int AmountofStacks = 3;
    public override void Start()
    {
        base.Start();
        BaseShield = 15; // Shield value that will be subtracted and depleted from the enemy's attack
    }

    // This effect creates a shield that will protect the player by this amount
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        PlayerDefenseUpEffect preExistantEffect = Player.GetComponent<PlayerDefenseUpEffect>();// Get the player's Decay Effect
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            PlayerDefenseUpEffect effectToAdd = Player.gameObject.AddComponent<PlayerDefenseUpEffect>() as PlayerDefenseUpEffect;// Apply a decay effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, AmountofStacks);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(AmountofStacks);// Add more stacks
        yield return StartCoroutine(base.CardEffect());
    }
}
