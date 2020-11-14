using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballOverflow : OverflowKeyword
{
    [SerializeField] public float DamagePercentage = .33f;// Deals 33% of the base damage
    public override void OverflowEffect()
    {
        Debug.Log("Overflowing");
        var RandomNumber = Random.Range(0, enemyManager.CombatEnemies.Count);
        var Target = enemyManager.CombatEnemies[RandomNumber];// Pick a random enemy
        Target.ProcessDamage(myCard.CalculateAction(Mathf.CeilToInt(myCard.PhysicalCardBehaviour.BaseDamage*DamagePercentage)));
    }
}
