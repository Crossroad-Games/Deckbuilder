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
        AlchemistFireEffect preExistantFireEffect = TargetEnemy.GetComponent<AlchemistFireEffect>();
        if (preExistantFireEffect == null)
        {
            Debug.Log(BaseDamage + " " + Divider);
            TargetEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));

            AlchemistFireEffect effectToAdd = TargetEnemy.gameObject.AddComponent<AlchemistFireEffect>() as AlchemistFireEffect;

            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, 1);
        }
        else
        {
            if(preExistantFireEffect.turnCounter % 2 ==0)
            {
                AddValue = preExistantFireEffect.turnCounter / 2;
            }
            TargetEnemy.ProcessDamage((BaseDamage + AddValue - SubtractValue) * ((int)(Multiplier / Divider)));
            preExistantFireEffect.turnCounter += 1;
        }
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
}
