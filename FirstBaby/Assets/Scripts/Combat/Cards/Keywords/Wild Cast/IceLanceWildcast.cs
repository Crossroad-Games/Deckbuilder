using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLanceWildcast : WildcastKeyword
{
    [SerializeField] public int IncapacitatedDuration = 1;
    [SerializeField] public float DamagePercentage = 0;// How much of the base damage is dealt when wild cast
    public override void WildCastEffect()
    {
        Debug.Log("Wildcast");
        var RandomNumber = Random.Range(0, enemyManager.CombatEnemies.Count);// Pick a random enemy
        targetEnemy = enemyManager.CombatEnemies[RandomNumber];// Acquire the random enemy class
        targetEnemy.ProcessDamage(myCard.CalculateAction(Mathf.CeilToInt(myCard.BaseDamage * DamagePercentage)));// Deals a % of the base damage to the enemy
        IncapacitatedEffect preExistantEffect = targetEnemy.GetComponent<IncapacitatedEffect>();
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            IncapacitatedEffect effectToAdd = targetEnemy.gameObject.AddComponent<IncapacitatedEffect>() as IncapacitatedEffect;// Apply a incapacitated effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, IncapacitatedDuration);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(IncapacitatedDuration);// Extend the incapacitation effect
    }
}
