using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballOverflow : OverflowKeyword
{
    [SerializeField] private float DamagePercentage = .33f;// Deals 33% of the base damage
    public override void OverflowEffect()
    {
        Debug.Log("Overflowing");
        var Target = enemyManager.CombatEnemies[Random.Range(0, enemyManager.CombatEnemies.Count)];// Pick a random enemy
        Target.ProcessDamage(myCard.CalculateAction(Mathf.CeilToInt(myCard.BaseDamage*DamagePercentage)));
    }
}
