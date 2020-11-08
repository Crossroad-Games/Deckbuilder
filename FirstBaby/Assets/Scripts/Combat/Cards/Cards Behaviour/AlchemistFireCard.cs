using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistFireCard : TargetCard
{
    // This effect deals damage to a single enemy
    public override IEnumerator CardEffect()
    {
        Debug.Log("alchemist fire effect");
        Agony preExistentAgony = TargetEnemy.GetComponent<Agony>();
        if (preExistentAgony == null)
        {
            Agony effectToAdd = TargetEnemy.gameObject.AddComponent<Agony>() as Agony;

            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, 1);
        }
        else
        {
            if(preExistentAgony.turnCounter % 2 ==0)
            {
                AddValue = preExistentAgony.turnCounter / 2;
            }
            preExistentAgony.turnCounter += 1;
        }
        effectFinished = true;
        yield return StartCoroutine(base.CardEffect());
    }
}
