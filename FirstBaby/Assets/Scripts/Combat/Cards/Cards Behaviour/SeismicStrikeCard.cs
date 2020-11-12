using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeismicStrikeCard : TargetCard
{
    [SerializeField] private int IncapacitatedDuration = 3;
    // This effect deals damage to a single enemy
    public override IEnumerator CardEffect()
    {
        effectFinished = true;
        IncapacitatedEffect preExistantEffect = TargetEnemy.GetComponent<IncapacitatedEffect>();
        if (preExistantEffect == null)// If there is no decay effect yet
        {

            IncapacitatedEffect effectToAdd = TargetEnemy.gameObject.AddComponent<IncapacitatedEffect>() as IncapacitatedEffect;// Apply a incapacitated effect
            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, IncapacitatedDuration);// Initialize the amount of stacks
        }
        else// If there is a decay effect
            preExistantEffect.AddStacks(IncapacitatedDuration);
        yield return StartCoroutine(base.CardEffect());
    }
}
