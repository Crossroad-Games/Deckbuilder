using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeismicStrikeOverflow : OverflowKeyword
{
    [SerializeField] private float DamagePercentage=1f;// How much of the base damage will this overflow deal?
    public override void OverflowEffect()
    {
        //Deal area damage
        foreach (EnemyClass Enemy in enemyManager.CombatEnemies)// Cycle through each enemy
            if (Enemy != null)// If enemy is not null
            {
                Debug.Log(myCard.CalculateAction(Mathf.CeilToInt(myCard.PhysicalCardBehaviour.BaseDamage * DamagePercentage)));
                Enemy.ProcessDamage(myCard.CalculateAction(Mathf.CeilToInt(myCard.PhysicalCardBehaviour.BaseDamage * DamagePercentage)));// Deal the base damage as overflow damage 
            }
    }
}
