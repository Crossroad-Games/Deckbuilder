using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistFireCard : TargetCard
{

    protected override void Awake()
    {
        base.Awake();
        BaseDamage = 5;
    }
    // This effect deals damage to a single enemy
    public override IEnumerator CardEffect()
    {
        Debug.Log("alchemist fire effect");
        AlchemistFireEffect preExistantFireEffect = TargetEnemy.GetComponent<AlchemistFireEffect>();
        if (preExistantFireEffect == null)
        {
            AlchemistFireEffect effectToAdd = TargetEnemy.gameObject.AddComponent<AlchemistFireEffect>() as AlchemistFireEffect;

            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, 1);
        }
        else
        {
            if(preExistantFireEffect.turnCounter % 2 ==0)
            {
                AddValue = preExistantFireEffect.turnCounter / 2;
            }
            preExistantFireEffect.turnCounter += 1;
        }
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
}
