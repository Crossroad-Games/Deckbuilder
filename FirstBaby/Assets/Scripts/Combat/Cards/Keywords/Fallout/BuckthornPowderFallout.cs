using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckthornPowderFallout : FalloutKeyword
{
    public override void FalloutEffect()
    {
        var Target = enemyManager.CombatEnemies[Random.Range(0, enemyManager.CombatEnemies.Count)];// Pick a random enemy
        Agony preExistentAgony = Target.GetComponent<Agony>();
        if (preExistentAgony != null)
        {
            if (preExistentAgony.turnCounter >= 4)
            {
                preExistentAgony.turnCounter -= 4;
                Target.ProcessDamage(myCard.CalculateAction(Mathf.CeilToInt(myCard.BaseDamage * 2f)));
            }
            else
            {
                preExistentAgony.turnCounter += 4;
            }
        }
        else
        {
            Agony effectToAdd = Target.gameObject.AddComponent<Agony>() as Agony;

            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, 4);
        }
        
    }
}
