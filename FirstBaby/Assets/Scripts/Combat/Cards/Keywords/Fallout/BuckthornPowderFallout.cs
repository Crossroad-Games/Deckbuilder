using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckthornPowderFallout : FalloutKeyword
{
    [SerializeField] public int AmountofStacks=4;
    [SerializeField] public float DamageMultiplier=2f;
    public override void FalloutEffect()
    {
        var Target = enemyManager.CombatEnemies[Random.Range(0, enemyManager.CombatEnemies.Count)];// Pick a random enemy
        Agony preExistentAgony = Target.GetComponent<Agony>();
        if (preExistentAgony != null)
        {
            if (preExistentAgony.turnCounter >= 4) // agony >= 4 stacks, deal double damage
            {
                preExistentAgony.AddStacks(-4);
                Target.ProcessDamage(myCard.CalculateAction(Mathf.CeilToInt(myCard.BaseDamage * DamageMultiplier)));
            }
            else //otherwise, deal +4 agony
            {
                preExistentAgony.AddStacks(AmountofStacks);
            }
        }
        else //if there was no instance of agony on the enemy, add the component then initialize it
        {
            Agony effectToAdd = Target.gameObject.AddComponent<Agony>() as Agony;

            effectToAdd.InitializeEffect(0, 0, 0, 0, 0, AmountofStacks);
        }
        
    }
}
